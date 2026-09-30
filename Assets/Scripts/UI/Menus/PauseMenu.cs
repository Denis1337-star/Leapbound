using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenu : ValidatedMonoBehaviour
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

    [Header("UI Text")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _statusText;

    [Header("Stars Display")]
    [SerializeField] private Transform _starParent;
    [SerializeField] private Image[] _stars;

    private bool _isPaused = false;

    private ISceneFlowService _sceneFlowService;
    private IScoreService _scoreService;
    private IGameStateService _gameStateService;

    [Inject]
    public void Construct(ISceneFlowService sceneFlowService, IScoreService scoreService,
        IGameStateService gameStateService)
    {
        _sceneFlowService = sceneFlowService;
        _scoreService = scoreService;
        _gameStateService = gameStateService;
    }
    protected override void Awake()
    {
        base.Awake();

        _pauseButton.onClick.AddListener(TogglePause);
        _restartButton.onClick.AddListener(RestartLevel);
        _mainMenuButton.onClick.AddListener(GoToMainMenu);
        _settingsButton.onClick.AddListener(OpenSettingsPanel);
    }
    private void OnEnable()
    {
        _gameStateService.OnStateChanged += HandleGameStateChanged;
        _scoreService.OnScoreChanged += HandleScoreChanged;
    }
    private void OnDisable()
    {
        _gameStateService.OnStateChanged -= HandleGameStateChanged;
        _scoreService.OnScoreChanged -= HandleScoreChanged;
    }
    protected override bool ValidateInternal()
    {
        bool valid = true;
        valid &= ValidationUtility.IsAssigned(this, _pausePanel, nameof(_pausePanel));
        valid &= ValidationUtility.IsAssigned(this, _pauseButton, nameof(_pauseButton));
        valid &= ValidationUtility.IsAssigned(this, _restartButton, nameof(_restartButton));
        valid &= ValidationUtility.IsAssigned(this, _mainMenuButton, nameof(_mainMenuButton));
        valid &= ValidationUtility.IsAssigned(this, _settingsButton, nameof(_settingsButton));
        valid &= ValidationUtility.IsAssigned(this, _scoreText, nameof(_scoreText));
        valid &= ValidationUtility.IsAssigned(this, _statusText, nameof(_statusText));
        valid &= ValidationUtility.IsAssigned(this, _starParent, nameof(_starParent));
        return valid;
    }
    private void Start()
    {
        _stars = _starParent.GetComponentsInChildren<Image>(true);
        HideAllStars();

        _pausePanel.SetActive(false);
        _nextLevelButton.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    private void UpdateUI()
    {
        _scoreText.text = $"Score: {_scoreService.Score}";

        switch (_gameStateService.CurrentState)
        {
            case GameState.Win:
                _statusText.text = "Ты победил";
                break;
            case GameState.Lose:
                _statusText.text = "Ты проиграл";
                break;
            case GameState.Paused:
                _statusText.text = "Пауза";
                break;
            default:
                _statusText.text = "Пауза";
                break;
        }

        if (_gameStateService.CurrentState == GameState.Win)
        {
            int stars = LevelStarCalculator.Calculate(_scoreService.Score);
            ShowStars(stars);
            _nextLevelButton.gameObject.SetActive(true);
        }
        else
        {
            HideAllStars();
            _nextLevelButton.gameObject.SetActive(false);
        }
    }

    private void ShowStars(int count)
    {
        for (int i = 0; i < _stars.Length; i++)
            _stars[i].enabled = i < count;
    }

    private void HideAllStars()
    {
        foreach (var star in _stars)
            star.enabled = false;
    }

    public void TogglePause()
    {

        if (_gameStateService.CurrentState == GameState.Win || _gameStateService.CurrentState == GameState.Lose)
        {
            UpdateUI();
            _isPaused = !_isPaused;
            _pausePanel.SetActive(_isPaused);

            if (_isPaused)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;

            return;
        }

        UpdateUI();
        _isPaused = !_isPaused;
        _pausePanel.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0f : 1f;
        _gameStateService.SetPaused(_isPaused);
    }

    public void RestartLevel()
    {
        _sceneFlowService.RestartCurrentLevel();
    }

    public void GoToMainMenu()
    {
        _sceneFlowService.LoadMainMenu();
    }

    public void OpenSettingsPanel()
    {
        _settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        _settingsPanel.SetActive(false);
    }

    public void LoadNextLevel()
    {
        _sceneFlowService.LoadNextLevel();
    }
    private void HandleScoreChanged(int newScore)
    {
        _scoreText.text = $"Счёт: {newScore}";
    }
    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.Win || newState == GameState.Lose)
        {
            UpdateUI();
            _isPaused = true;
            _pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
