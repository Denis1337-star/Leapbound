using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
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

    [Header("Settings Sliders")]
    [SerializeField] private Slider _musicSlider;  
    [SerializeField] private Slider _sfxSlider;

    private bool _isPaused = false;  

    private void Start()
    {
        // Настройка звёзд
        _stars = _starParent.GetComponentsInChildren<Image>(true);  //получает все звезды
        HideAllStars();  //скрывает звезды

        // Скрыть панели в начале
        _pausePanel.SetActive(false);  
        if (_nextLevelButton != null) _nextLevelButton.gameObject.SetActive(false);

        // Подписка кнопок
        _pauseButton.onClick.AddListener(TogglePause);
        _restartButton.onClick.AddListener(RestartLevel);
        _mainMenuButton.onClick.AddListener(GoToMainMenu);
        _settingsButton.onClick.AddListener(OpenSettingsPanel);

        // Настройка слайдеров
        if (SettingsManager.Instance != null)
        {
            _musicSlider.value = SettingsManager.Instance.GetMusicValue();
            _sfxSlider.value = SettingsManager.Instance.GetSFXValue();

            _musicSlider.onValueChanged.AddListener(SettingsManager.Instance.SetMusicValue);
            _sfxSlider.onValueChanged.AddListener(SettingsManager.Instance.SetSFXValue);
        }

        Time.timeScale = 1f; // в начале игра не на паузе
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();

        if (_pausePanel.activeSelf)
            UpdateUI();
    }

    private void UpdateUI()
    {
        // Обновление текста очков
        _scoreText.text = $"Score: {PlayerScore.Score}";

        if (PlayerScore.IsGameOver)
        {
            _statusText.text = PlayerScore.IsWin ? "YOU WIN!" : "YOU LOSE!";
        }
        else
        {
            _statusText.text = "PAUSED";
        }

        if (PlayerScore.IsWin)
        {
            int stars = CalculateStars(PlayerScore.Score);
            ShowStars(stars);

            if (_nextLevelButton)
                _nextLevelButton.gameObject.SetActive(true);
        }
    }

    private void ShowStars(int count)
    { 
        //включает звезды 
        for (int i = 0; i < _stars.Length; i++)
            _stars[i].enabled = i < count;
    }

    private void HideAllStars()
    {  
        //проходит по массиву и откл звезды
        foreach (var star in _stars)
            star.enabled = false;
    }

    public void TogglePause()
    {
        //режим паузы 
        _isPaused = !_isPaused;
        _pausePanel.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0f : 1f;
    }

    public void RestartLevel()
    {
        //перезапуск уровня
        Time.timeScale = 1f;
        PlayerScore.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    { 
        //в главное меню
        Time.timeScale = 1f;
        PlayerScore.Reset();
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSettingsPanel()
    {
        if (_settingsPanel != null)
            _settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        if (_settingsPanel != null)
            _settingsPanel.SetActive(false);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        PlayerScore.Reset();

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            SceneManager.LoadScene("MainMenu"); ;
    }
    private int CalculateStars(int score)
    {
        if (score >= 100)
            return 3;

        if (score >= 50)
            return 2;

        return 1;
    }
}
