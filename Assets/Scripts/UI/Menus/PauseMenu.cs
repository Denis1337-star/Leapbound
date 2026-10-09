using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private Image[] _stars;

    private ISceneFlowService _sceneFlowService;
    private IScoreService _scoreService;
    private IGameStateService _gameStateService;
    private IInputService _inputService;

    [Inject]
    public void Construct(ISceneFlowService sceneFlowService, IScoreService scoreService,
        IGameStateService gameStateService, IInputService inputService)
    {
        _sceneFlowService = sceneFlowService;
        _scoreService = scoreService;
        _gameStateService = gameStateService;
        _inputService = inputService;
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
    }

    private void HandleGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Win:
                _statusText.SetText("ТЫ ПОБЕДИЛ");
                ShowStars(LevelStarCalculator.Calculate(_scoreService.Score));
                SetNextButtonVisiale(true);
                SetPausePanelVisible(true);
                SetGameFrozen(true);
                break;

            case GameState.Lose:
                _statusText.SetText("ТЫ ПРОИГРАЛ");
                ShowStars(0);
                SetNextButtonVisiale(false);
                SetPausePanelVisible(true);
                SetGameFrozen(true);
                break;

            case GameState.Paused:
                _statusText.SetText("ПАУЗА");
                SetPausePanelVisible(true);
                SetGameFrozen(true);
                break;

            case GameState.Playing:
                _statusText.SetText(string.Empty);
                SetPausePanelVisible(false);
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
            _gameStateService.SetPaused(true);
    }
    private void ResetAllUI()
    {
        SetGameFrozen(false);
        SetPausePanelVisible(false);
        SetNextButtonVisiale(false);
        ShowStars(0);

    }
    private void UpdateScoreText(int newScore)
    {
        _scoreText.SetText("Счёт: {0}", newScore);
    }
    private void ShowStars(int count)
    {
        for (int i = 0; i < _stars.Length; i++)
            _stars[i].enabled = i < count;
    }
    private void SetPausePanelVisible(bool visible) { _pausePanel.SetActive(visible); }
    private void SetNextButtonVisiale(bool visible) { _nextLevelButton.gameObject.SetActive(visible); }
    private void SetGameFrozen(bool frozen)
    {
        if (frozen)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    private void BindButtons()
    {
        _pauseButton.onClick.AddListener(TogglePause);
        _restartButton.onClick.AddListener(() => _sceneFlowService.RestartCurrentLevel());
        _mainMenuButton.onClick.AddListener(() => _sceneFlowService.LoadMainMenu());
        _nextLevelButton.onClick.AddListener(() => _sceneFlowService.LoadNextLevel());
    }
}
