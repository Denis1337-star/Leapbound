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
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _statusText;

    [Header("Stars Display")]
    [SerializeField] private Transform _starParent;
    [SerializeField] private Image[] _stars;

    private bool _isPaused = false;

    private ISceneFlowService _sceneFlowService;
    private IScoreService _scoreService;

    [Inject]
    public void Construct(ISceneFlowService sceneFlowService, IScoreService scoreService)
    {
        _sceneFlowService = sceneFlowService;
        _scoreService = scoreService;
    }
    protected override void Awake()
    {
        base.Awake();

        _pauseButton.onClick.AddListener(TogglePause);
        _restartButton.onClick.AddListener(RestartLevel);
        _mainMenuButton.onClick.AddListener(GoToMainMenu);
        _settingsButton.onClick.AddListener(OpenSettingsPanel);
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

        switch (_scoreService.State)
        {
            case GameState.Won:
                _statusText.text = "Ты выйиграл";
                break;
            case GameState.Lost:
                _statusText.text = "Ты пройиграл";
                break;
            case GameState.Paused:
                _statusText.text = "Пауза";
                break;
            default:
                _statusText.text = "Пауза";
                break;


        }

        if (_scoreService.State == GameState.Won)
        {
            int stars = LevelStarCalculator.Calculate(_scoreService.Score);
            ShowStars(stars);

            _nextLevelButton.gameObject.SetActive(true);
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
        if (_scoreService.State == GameState.Won)
        {
            UpdateUI();
            _isPaused = !_isPaused;
            _pausePanel.SetActive(_isPaused);
            Time.timeScale = _isPaused ? 0f : 1f;
            return;
        }

        UpdateUI();
        _isPaused = !_isPaused;
        _pausePanel.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0f : 1f;

        if (_scoreService.State != GameState.Won && _scoreService.State != GameState.Lost)
            _scoreService.SetPaused(_isPaused);
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
}
