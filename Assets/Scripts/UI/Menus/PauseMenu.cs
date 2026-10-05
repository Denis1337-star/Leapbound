using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenu :MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _settingsPanel;

    [Header("Pause Menu Buttons")]
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _closeSettingButtom;

    [Header("UI Text")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _statusText;

    [Header("Stars Display")]
    [SerializeField] private Transform _starParent;
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
                int stars = LevelStarCalculator.Calculate(_scoreService.Score);
                ShowStars(stars);
                SetNextButtonVisiale(true);
                SetPausePanelVisible(true);
                SetGameFrozen(true);
                break;

            case GameState.Lose:
                _statusText.SetText("ТЫ ПРОИГРАЛ");
                ResetAllUI();
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
        {
            SetPausePanelVisible(!_pausePanel.activeSelf);
            return;
        }

        if (currentGameState == GameState.Playing)
            _gameStateService.SetPaused(paused: true);
        else if (currentGameState == GameState.Paused)
            _gameStateService.SetPaused(paused: true);
    }
    private void ResetAllUI()
    {
        SetGameFrozen(false);
        SetPausePanelVisible(false);
        SetSettingPanelVisiable(false);
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
    private void SetPausePanelVisible(bool visiable)
    {
        _pausePanel.SetActive(visiable);
        if (visiable)
            SetPausePanelVisible(false);
    }
    private void SetSettingPanelVisiable(bool visiable) { _settingsPanel.SetActive(visiable); }
    private void SetNextButtonVisiale(bool visiable) { _nextLevelButton.gameObject.SetActive(visiable); }
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
        _settingsButton.onClick.AddListener(OpenSettingsPanel);
        _closeSettingButtom.onClick.AddListener(CloseSettingsPanel);
    }
    public void CloseSettingsPanel() { _settingsPanel.SetActive(false); }
    public void OpenSettingsPanel() { _settingsPanel.SetActive(true); }
}
