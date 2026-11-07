using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _rewardLifeButton;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private Image[] _stars;

    private ISceneFlowService _sceneFlowService;
    private IScoreService _scoreService;
    private IGameStateService _gameStateService;
    private IInputService _inputService;
    private LevelStarCalculator _levelStarCalculator;
    private PlayerLives _playerLives;
    private PlayerRespawn _playerRespawn;

    [Inject]
    public void Construct(ISceneFlowService sceneFlowService, IScoreService scoreService,
        IGameStateService gameStateService, IInputService inputService,
        LevelStarCalculator levelStarCalculator, PlayerLives playerLives, PlayerRespawn playerRespawn)
    {
        _sceneFlowService = sceneFlowService;
        _scoreService = scoreService;
        _gameStateService = gameStateService;
        _inputService = inputService;
        _levelStarCalculator = levelStarCalculator;
        _playerLives = playerLives;
        _playerRespawn = playerRespawn;
    }
    private void Awake()
    {
        BindButtons();
    }
    private void Start()
    {
        ResetAllUI();
        UpdateScoreText(_scoreService.Score);
    }
    private void OnEnable()
    {
        _gameStateService.OnStateChanged += HandleGameStateChanged;
        _scoreService.OnScoreChanged += UpdateScoreText;
        _inputService.PausedRequested += TogglePause;
    }
    private void OnDisable()
    {
        _gameStateService.OnStateChanged -= HandleGameStateChanged;
        _scoreService.OnScoreChanged -= UpdateScoreText;
        _inputService.PausedRequested -= TogglePause;
        YG2.onCloseAnyAdv -= LoadNextLevelAfterAd;
        YG2.onCloseAnyAdv -= LoadMainMenuAfterAd;
        YG2.onCloseRewardedAdv -= UnfreezeAfterRewardedAd;
    }
    private void HandleGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Win:
                _statusText.SetText("Ты победил");
                ShowStars(_levelStarCalculator.Calculate(_scoreService.Score));
                SetPausePanelVisible(true);
                SetGameFrozen(true);
                RefreshActionSlot();
                break;

            case GameState.Lose:
                _statusText.SetText("Попытки закончились");
                ShowStars(0);
                SetPausePanelVisible(true);
                SetGameFrozen(true);
                RefreshActionSlot();
                break;

            case GameState.Paused:
                _statusText.SetText("Пауза");
                SetPausePanelVisible(true);
                SetGameFrozen(true);
                RefreshActionSlot();
                break;

            case GameState.Playing:
                _statusText.SetText(string.Empty);
                SetPausePanelVisible(false);
                SetGameFrozen(false);
                RefreshActionSlot();
                break;
        }
    }
    public void TogglePause()
    {
        GameState currentGameState = _gameStateService.CurrentState;
        if (currentGameState == GameState.Win || currentGameState == GameState.Lose)
            return;

        if (currentGameState == GameState.Playing)
            _gameStateService.SetPaused(true);
        else if (currentGameState == GameState.Paused)
            _gameStateService.SetPaused(false);
    }
    private void ResetAllUI()
    {
        SetGameFrozen(false);
        SetPausePanelVisible(false);
        ShowStars(0);
        RefreshActionSlot();
    }
    private void UpdateScoreText(int newScore){_scoreText.SetText("{0}", newScore);}
    private void ShowStars(int count)
    {
        for (int i = 0; i < _stars.Length; i++)
            _stars[i].enabled = i < count;
    }
    private void SetPausePanelVisible(bool visible) { _pausePanel.SetActive(visible); }
    private void SetGameFrozen(bool frozen)
    {
        if (frozen)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }
    private void RefreshActionSlot()
    {
        GameState state = _gameStateService.CurrentState;
        bool showNext = state == GameState.Win;
        bool showRewardLife = state == GameState.Lose;

        _nextLevelButton.gameObject.SetActive(showNext);
        _rewardLifeButton.gameObject.SetActive(showRewardLife);
    }
    private void BindButtons()
    {
        _pauseButton.onClick.AddListener(TogglePause);
        _restartButton.onClick.AddListener(() => _sceneFlowService.RestartCurrentLevel());
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        _nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        _rewardLifeButton.onClick.AddListener(OnRewardLifeButtonClicked);
    }
    private void OnNextLevelButtonClicked()
    {
        if (_gameStateService.CurrentState != GameState.Win)
        {
            _sceneFlowService.LoadNextLevel();
            return;
        }

        YG2.onCloseAnyAdv += LoadNextLevelAfterAd;
        YG2.InterstitialAdvShow();
    }
    private void OnMainMenuButtonClicked()
    {
        if (_gameStateService.CurrentState != GameState.Win)
        {
            _sceneFlowService.LoadMainMenu();
            return;
        }

        YG2.onCloseAnyAdv += LoadMainMenuAfterAd;
        YG2.InterstitialAdvShow();
    }
    private void OnRewardLifeButtonClicked()
    {
        if (_gameStateService.CurrentState != GameState.Lose)
            return;

        YG2.onCloseRewardedAdv += UnfreezeAfterRewardedAd;
        YG2.RewardedAdvShow("extra_life", GrantExtraLifeAfterReward);
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
        YG2.onCloseAnyAdv -= LoadNextLevelAfterAd;
        _sceneFlowService.LoadNextLevel();
    }
    private void LoadMainMenuAfterAd()
    {
        YG2.onCloseAnyAdv -= LoadMainMenuAfterAd;
        _sceneFlowService.LoadMainMenu();
    }
    private void UnfreezeAfterRewardedAd()
    {
        YG2.onCloseRewardedAdv -= UnfreezeAfterRewardedAd;
        if (_gameStateService.CurrentState != GameState.Playing)
            return;

        PauseGameYG.SetState(1f, false, true);
    }
}