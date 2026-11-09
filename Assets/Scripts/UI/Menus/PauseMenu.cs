using UnityEngine;
using UnityEngine.UI;
using Zenject;

public sealed class PauseMenu : MonoBehaviour
{
    [SerializeField] private PauseMenuView _view;
    [SerializeField] private Button _pauseButton;

    private ISceneFlowService _sceneFlowService;
    private IScoreService _scoreService;
    private IGameStateService _gameStateService;
    private IInputService _inputService;
    private LevelStarCalculator _levelStarCalculator;
    private PlayerLives _playerLives;
    private PlayerRespawn _playerRespawn;
    private PauseMenuAds _ads;
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
        _ads = new PauseMenuAds(_gameStateService, _sceneFlowService, _playerLives, _playerRespawn,
            _view.StopRewardPulsePublic);
        BindButtons();
    }
    private void Start()
    {
        _view.ShowPlaying();
        SetGameFrozen(false);
        _view.SetScore(_scoreService.Score);
    }
    private void OnEnable()
    {
        _gameStateService.OnStateChanged += HandleGameStateChanged;
        _scoreService.OnScoreChanged += _view.SetScore;
        _inputService.PausedRequested += TogglePause;
    }
    private void OnDisable()
    {
        _gameStateService.OnStateChanged -= HandleGameStateChanged;
        _scoreService.OnScoreChanged -= _view.SetScore;
        _inputService.PausedRequested -= TogglePause;
        _ads.Dispose();
    }
    private void HandleGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Win:
                _view.ShowStars(_levelStarCalculator.Calculate(_scoreService.Score));
                _view.ShowWin();
                _view.SetReviewPromptVisible(_gameReviewPrompt.CanShowSoftPrompt());
                SetGameFrozen(true);
                break;
            case GameState.Lose:
                _view.ShowLose();
                SetGameFrozen(true);
                break;
            case GameState.Paused:
                _view.ShowPaused();
                SetGameFrozen(true);
                break;
            case GameState.Playing:
                _view.ShowPlaying();
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
        Time.timeScale = frozen ? 0f : 1f;
    }
    private void OnReviewAccept()
    {
        _view.SetReviewPromptVisible(false);
        _gameReviewPrompt.Accept();
    }
    private void OnReviewDecline()
    {
        _view.SetReviewPromptVisible(false);
        _gameReviewPrompt.Decline();
    }
    private void BindButtons()
    {
        _pauseButton.onClick.AddListener(TogglePause);
        _view.CloseButton.onClick.AddListener(TogglePause);
        _view.RestartButton.onClick.AddListener(() => _sceneFlowService.RestartCurrentLevel());
        _view.MainMenuButton.onClick.AddListener(_ads.OnMainMenuClicked);
        _view.NextLevelButton.onClick.AddListener(_ads.OnNextLevelClicked);
        _view.RewardLifeButton.onClick.AddListener(_ads.OnRewardLifeClicked);
        _view.ReviewAcceptButton.onClick.AddListener(OnReviewAccept);
        _view.ReviewDeclineButton.onClick.AddListener(OnReviewDecline);
    }
}