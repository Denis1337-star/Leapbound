using System;
using YG;

public sealed class PauseMenuAds
{
    private readonly IGameStateService _gameStateService;
    private readonly ISceneFlowService _sceneFlowService;
    private readonly PlayerLives _playerLives;
    private readonly PlayerRespawn _playerRespawn;
    private readonly LevelRunContext _levelRunContext;
    private readonly Action _onRewardStarted;
    private readonly LevelResultManager _levelResultManager;
    private readonly LevelTimeCatalog _levelTimeCatalog;
    private Action _afterInterstitial;

    public PauseMenuAds(IGameStateService gameStateService, ISceneFlowService sceneFlowService,
        PlayerLives playerLives, PlayerRespawn playerRespawn, Action onRewardStarted,
        LevelRunContext levelRunContext, LevelTimeCatalog levelTimeCatalog, LevelResultManager levelResultManager)
    {
        _gameStateService = gameStateService;
        _sceneFlowService = sceneFlowService;
        _playerLives = playerLives;
        _playerRespawn = playerRespawn;
        _onRewardStarted = onRewardStarted;
        _levelRunContext = levelRunContext;
        _levelTimeCatalog = levelTimeCatalog;
        _levelResultManager = levelResultManager;
    }
    public void Dispose()
    {
        YG2.onCloseAnyAdv -= OnInterstitialClosed;
        YG2.onCloseRewardedAdv -= UnfreezeAfterRewardedAd;
        _afterInterstitial = null;
    }
    public void OnNextLevelClicked()
    {
        if (_gameStateService.CurrentState != GameState.Win)
        {
            PrepareNextLevelRunMode();
            _sceneFlowService.LoadNextLevel();
            return;
        }
        ShowInterstitialThen(LoadNextLevelAfterAd);
    }
    public void OnMainMenuClicked()
    {
        if (_gameStateService.CurrentState != GameState.Win)
        {
            _levelRunContext.SetNormal();
            _sceneFlowService.LoadMainMenu();
            return;
        }
        ShowInterstitialThen(LoadMainMenuAfterAd);
    }
    public void OnRewardLifeClicked()
    {
        if (_gameStateService.CurrentState != GameState.Lose)
            return;

        _onRewardStarted?.Invoke();
        YG2.onCloseRewardedAdv += UnfreezeAfterRewardedAd;
        YandexRewardedBridge.Show("extra_life", GrantExtraLifeAfterReward);
    }
    private void GrantExtraLifeAfterReward()
    {
        _playerLives.AddLife(1);
        _playerRespawn.Respawn();
        _gameStateService.Reset();
        PauseGameYG.SetState(1f, false, true);
    }
    private void LoadNextLevelAfterAd()
    {
        PrepareNextLevelRunMode();
        _sceneFlowService.LoadNextLevel();
    }
    private void LoadMainMenuAfterAd()
    {
        _levelRunContext.SetNormal();
        _sceneFlowService.LoadMainMenu();
    }
    private void UnfreezeAfterRewardedAd()
    {
        YG2.onCloseRewardedAdv -= UnfreezeAfterRewardedAd;
        if (_gameStateService.CurrentState != GameState.Playing)
            return;

        PauseGameYG.SetState(1f, false, true);
    }
    private void PrepareNextLevelRunMode()
    {
        if (_levelRunContext.LevelMode != LevelRunMode.TimeTrial)
        {
            _levelRunContext.SetNormal();
            return;
        }

        int nextIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            _levelRunContext.SetNormal();
            return;
        }

        if (!_levelResultManager.CanStartTimeTrial(nextIndex))
        {
            _levelRunContext.SetNormal();
            return;
        }

        float target = _levelTimeCatalog.GetTargetTimeSeconds(nextIndex);
        _levelRunContext.SetTimeTrial(target);
    }
    public void OnTimeTrialOfferClicked()
    {
        _levelRunContext.RequestOpenLevelsOnMainMenu();
        if (_gameStateService.CurrentState != GameState.Win)
        {
            _levelRunContext.SetNormal();
            _sceneFlowService.LoadMainMenu();
            return;
        }
        ShowInterstitialThen(LoadMainMenuAfterAd);
    }
    private void ShowInterstitialThen(Action afterClose)
    {
        _afterInterstitial = afterClose;
        YG2.onCloseAnyAdv += OnInterstitialClosed;
        YG2.InterstitialAdvShow();
    }
    private void OnInterstitialClosed()
    {
        YG2.onCloseAnyAdv -= OnInterstitialClosed;
        Action after = _afterInterstitial;
        _afterInterstitial = null;

        if (after != null)
            after();
    }
}