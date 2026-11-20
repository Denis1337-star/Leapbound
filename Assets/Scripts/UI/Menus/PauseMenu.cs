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

    [Inject]
    public void Construct(ISceneFlowService sceneFlowService, IScoreService scoreService,
        IGameStateService gameStateService, IInputService inputService,
        LevelStarCalculator levelStarCalculator, PlayerLives playerLives, PlayerRespawn playerRespawn, GameReviewPrompt gameReviewPrompt)
    {
        _sceneFlowService = sceneFlowService;
        _scoreService = scoreService;
        _gameStateService = gameStateService;
        _inputService = inputService;
        _levelStarCalculator = levelStarCalculator;
        _playerLives = playerLives;
        _playerRespawn = playerRespawn;
        _gameReviewPrompt = gameReviewPrompt;
    }
    private void Awake()
    {
        _pauseMenuAds = new PauseMenuAds(_gameStateService, _sceneFlowService, _playerLives, _playerRespawn,
            _pauseMenuView.StopRewardPulsePublic);
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
        switch (newState)
        {
            case GameState.Win:
                RefreshStars(true);
                _pauseMenuView.ShowWin();
                _pauseMenuView.SetReviewPromptVisible(_gameReviewPrompt.CanShowSoftPrompt());
                SetGameFrozen(true);
                break;
            case GameState.Lose:
                RefreshStars(false);
                _pauseMenuView.ShowLose();
                SetGameFrozen(true);
                break;
            case GameState.Paused:
                RefreshStars(false);
                _pauseMenuView.ShowPaused();
                SetGameFrozen(true);
                break;
            case GameState.Playing:
                _pauseMenuView.ShowPlaying();
                SetGameFrozen(false);
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
    private void BindButtons()
    {
        _pauseButton.onClick.AddListener(TogglePause);
        _pauseMenuView.CloseButton.onClick.AddListener(TogglePause);
        _pauseMenuView.RestartButton.onClick.AddListener(() => _sceneFlowService.RestartCurrentLevel());
        _pauseMenuView.MainMenuButton.onClick.AddListener(_pauseMenuAds.OnMainMenuClicked);
        _pauseMenuView.NextLevelButton.onClick.AddListener(_pauseMenuAds.OnNextLevelClicked);
        _pauseMenuView.RewardLifeButton.onClick.AddListener(_pauseMenuAds.OnRewardLifeClicked);
        _pauseMenuView.ReviewAcceptButton.onClick.AddListener(OnReviewAccept);
        _pauseMenuView.ReviewDeclineButton.onClick.AddListener(OnReviewDecline);
    }
    private void RefreshStars(bool finished)
    {
        LevelStarResult result = _levelStarCalculator.Calculate(
            _scoreService.Score, _playerLives.HasLostLife, finished);
        _pauseMenuView.ApplyStars(result, _scoreService.Score, _levelStarCalculator.TargetScore, finished);
    }
}