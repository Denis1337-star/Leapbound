using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausePanel;  //основная панель
    public GameObject settingsPanel;  //настроект fdfd
    
    [Header("Pause Menu Buttons")]
    public Button pauseButton;  //пауза\продолжение
    public Button restartButton;  //перезапск
    public Button mainMenuButton;  //выход в главное меню
    public Button settingsButton;  //настройки
    public Button nextLevelButton;  //след уровень

    [Header("UI Text")]
    public Text scoreText;  //счет
    public Text statusText; //вийграл или нет

    [Header("Stars Display")]
    public Transform starParent;  //родитель для звезд
    private Image[] stars;  //массив звезд

    [Header("Settings Sliders")]
    public Slider musicSlider;  //слайдеры для настройки громкости
    public Slider sfxSlider;

    private bool isPaused = false;  //флаг по паузе

    private void Start()
    {
        // Настройка звёзд
        stars = starParent.GetComponentsInChildren<Image>(true);  //получает все звезды
        HideAllStars();  //скрывает звезды

        // Скрыть панели в начале
        pausePanel.SetActive(false);  
        if (nextLevelButton != null) nextLevelButton.gameObject.SetActive(false);

        // Подписка кнопок
        pauseButton.onClick.AddListener(TogglePause);
        restartButton.onClick.AddListener(RestartLevel);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        settingsButton.onClick.AddListener(OpenSettingsPanel);

        // Настройка слайдеров
        if (SettingsManager.Instance != null)
        {
            musicSlider.value = SettingsManager.Instance.GetMusicValue();
            sfxSlider.value = SettingsManager.Instance.GetSFXValue();

            musicSlider.onValueChanged.AddListener(v => SettingsManager.Instance.SetMusicValue(v));
            sfxSlider.onValueChanged.AddListener(v => SettingsManager.Instance.SetSFXValue(v));
        }

        Time.timeScale = 1f; // в начале игра не на паузе
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();

        if (pausePanel.activeSelf)
            UpdateUI();
    }

    private void UpdateUI()
    {
        // Обновление текста очков
        scoreText.text = $"Score: {PlayerScore.CurrentScore}";

        statusText.text = PlayerScore.IsGameOver
            ? (PlayerScore.IsWin ? "You Win!" : "You Lose!")
            : "Paused";

        if (PlayerScore.IsWin)
        { 
            LevelResultManager.SaveResult(SceneManager.GetActiveScene().name,  //сохраняет результат
                                          SceneManager.GetActiveScene().buildIndex + 1);
            ShowStars(LevelResultManager.starsEarned);  //показывает звезды

            if (nextLevelButton != null)
                nextLevelButton.gameObject.SetActive(true);  //активирует кнопку некс лвл
        }
        else
        {
            HideAllStars(); //откл звезды
        }
    }

    private void ShowStars(int count)
    { 
        //включает звезды 
        for (int i = 0; i < stars.Length; i++)
            stars[i].enabled = i < count;
    }

    private void HideAllStars()
    {  
        //проходит по массиву и откл звезды
        foreach (var star in stars)
            star.enabled = false;
    }

    public void TogglePause()
    {
        //режим паузы 
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
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
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void GuidePanel()
    {
        Time.timeScale = 1f;
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        PlayerScore.Reset();

        //задает хар для некс уровня
        int currentIndex = SceneManager.GetActiveScene().buildIndex; 
        int nextIndex = currentIndex + 1;


        //если нету след уровня уходит в главное меню
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            SceneManager.LoadScene("MainMenu");
    }


}
