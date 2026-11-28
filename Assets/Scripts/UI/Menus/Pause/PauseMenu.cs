using UnityEngine;
using UnityEngine.UI;
using Zenject;

public sealed class PauseMenu : MonoBehaviour
{
    [SerializeField] private PauseMenuView _pauseMenuView;
    [SerializeField] private Button _pauseButton;

    private ISceneFlowService _sceneFlowService;
    private IScoreService _scoreService;
    private IGameStateService _gameStateService;
    private IInputService _inputService;
    private LevelStarCalculator _levelStarCalculator;
    private PlayerLives _playerLives;
    private PlayerRespawn _playerRespawn;
    private PauseMenuAds _pauseMenuAds;
    private GameReviewPrompt _gameReviewPrompt;
    private LevelRunContext _levelRunContext;
    private LevelTimer _levelTimer;
    private LevelResultManager _levelResultManager;
    private LevelTimeCatalog _levelTimeCatalog;
    private LevelEndPresenter _levelEndPresenter;
    private HeroProgress _heroProgress;
    private AnalyticsService _analyticsService;
    private LevelSessionStats _levelSessionStats;

    [Inject]
    public void Construct(ISceneFlowService sceneFlowService, IScoreService scoreService,
        IGameStateService gameStateService, IInputService inputService,LevelStarCalculator 
        levelStarCalculator, PlayerLives playerLives, PlayerRespawn playerRespawn,GameReviewPrompt gameReviewPrompt,
        LevelRunContext levelRunContext, LevelTimer levelTimer, LevelResultManager levelResultManager, 
        LevelTimeCatalog levelTimeCatalog, HeroProgress heroProgress, AnalyticsService analyticsService, 
        LevelSessionStats levelSessionStats)
    {
        _sceneFlowService = sceneFlowService;
        _scoreService = scoreService;
        _gameStateService = gameStateService;
        _inputService = inputService;
        _levelStarCalculator = levelStarCalculator;
        _playerLives = playerLives;
        _playerRespawn = playerRespawn;
        _gameReviewPrompt = gameReviewPrompt;
        _levelRunContext = levelRunContext;
        _levelTimer = levelTimer;
        _levelResultManager = levelResultManager;
        _levelTimeCatalog = levelTimeCatalog;
        _heroProgress = heroProgress;
        _analyticsService = analyticsService;
        _levelSessionStats = levelSessionStats;
    }
    private void Awake()
    {
        _pauseMenuAds = new PauseMenuAds(_gameStateService, _sceneFlowService, _playerLives, _playerRespawn,
            _pauseMenuView.StopRewardPulse, _levelRunContext, _levelTimeCatalog, _levelResultManager, _heroProgress, 
            _analyticsService, _levelSessionStats);

        _levelEndPresenter = new LevelEndPresenter(_levelRunContext, _levelTimer, _levelStarCalculator,
            _scoreService, _playerLives, _pauseMenuView, _gameReviewPrompt, _sceneFlowService, _analyticsService, _levelSessionStats);
        BindButtons();
    }
    private void Start()
    {
        _pauseMenuView.ShowPlaying();
        SetGameFrozen(false);
        _pauseMenuView.SetScore(_scoreService.Score);
    }
    private void OnEnable()
    {
        _gameStateService.OnStateChanged += HandleGameStateChanged;
        _scoreService.OnScoreChanged += _pauseMenuView.SetScore;
        _inputService.PausedRequested += TogglePause;
    }
    private void OnDisable()
    {
        _gameStateService.OnStateChanged -= HandleGameStateChanged;
        _scoreService.OnScoreChanged -= _pauseMenuView.SetScore;
        _inputService.PausedRequested -= TogglePause;
        _pauseMenuAds.Dispose();
    }
    private void HandleGameStateChanged(GameState newState)
    {
        _levelEndPresenter.Apply(newState, SetGameFrozen);
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
    private void SetGameFrozen(bool frozen)
    {
        if (frozen)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }
    private void OnReviewAccept()
    {
        _pauseMenuView.SetReviewPromptVisible(false);
        _gameReviewPrompt.Accept();
    }
    private void OnReviewDecline()
    {
        _pauseMenuView.SetReviewPromptVisible(false);
        _gameReviewPrompt.Decline();
    }
    private void OnRestartClicked()
    {
        _analyticsService.LevelRestart(_levelSessionStats.LevelBuildIndex,AnalyticsLabels.RunMode(_levelRunContext),
            _levelSessionStats.Deaths,_levelSessionStats.ElapsedSecondsRounded);

        _heroProgress.ResetToKnightIfNeeded(_levelRunContext);
        _sceneFlowService.RestartCurrentLevel();
    }
    private void BindButtons()
    {
        _pauseButton.onClick.AddListener(TogglePause);
        _pauseMenuView.CloseButton.onClick.AddListener(TogglePause);
        _pauseMenuView.RestartButton.onClick.AddListener(OnRestartClicked);
        _pauseMenuView.MainMenuButton.onClick.AddListener(_pauseMenuAds.OnMainMenuClicked);
        _pauseMenuView.NextLevelButton.onClick.AddListener(_pauseMenuAds.OnNextLevelClicked);
        _pauseMenuView.RewardLifeButton.onClick.AddListener(_pauseMenuAds.OnRewardLifeClicked);
        _pauseMenuView.ReviewAcceptButton.onClick.AddListener(OnReviewAccept);
        _pauseMenuView.ReviewDeclineButton.onClick.AddListener(OnReviewDecline);
        _pauseMenuView.TimeTrialOfferButton.onClick.AddListener(_pauseMenuAds.OnTimeTrialOfferClicked);
    }
}