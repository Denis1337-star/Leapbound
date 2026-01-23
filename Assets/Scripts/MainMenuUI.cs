using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [System.Serializable]
    public class LevelButton
    {
        public string levelName;  //имя сцены
        public int levelIndex;  //порядковый номер уровня
        public Button button;  //кнопка от уровня
        public Image[] stars;  //массив для отображения звезд
    }

    public LevelButton[] levelButtons;  //массив для кнопок уровня(для каждого задается из выше класса)
    public Button playButton;  //кнопка играть
    public Button settingsButton;  //открывет настройки
    public GameObject settingsPanel;  //панель настроек

    private void Start()
    {
        playButton.onClick.AddListener(PlayLastLevel);  //накидывает методы на кнопки
        settingsButton.onClick.AddListener(OpenSettings);

        UpdateLevelButtons();  //инициализирует UI
    }

    private void UpdateLevelButtons()
    {
        foreach (var lvl in levelButtons)  //проходит по массиву(классу)
        {
            bool unlocked = LevelResultManager.IsLevelUnlocked(lvl.levelIndex); //true если уровень открыт 
            lvl.button.interactable = unlocked;  //за актив кнопки отвечает

            int count = LevelResultManager.GetStarsForLevel(lvl.levelName);  //получает количество звезд заработаных за лвл
            for (int i = 0; i < lvl.stars.Length; i++)  //проходит по массиву 
                lvl.stars[i].enabled = i < count;  //включает столько звезд сколько надо

            lvl.button.onClick.RemoveAllListeners();  //защита от дублирования 
            if (unlocked)  //если открыт лвл 
                lvl.button.onClick.AddListener(() => LoadLevel(lvl.levelName));  //добавляет метод на запуск нужного уровня 
        }
    }

    private void PlayLastLevel()
    {
        int lastUnlocked = LevelResultManager.GetLastUnlockedLevel();  //получает индекс последнего разблокировано уровня
        foreach (var lvl in levelButtons)  //проходит по массиву в поиске
        {
            if (lvl.levelIndex == lastUnlocked)  //если лвл открыт 
            {
                LoadLevel(lvl.levelName);  //загружает сцену с этим именем
                return;
            }
        }
     
    }

    private void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);  //запускает сцену по имени 
    }

    private void OpenSettings()
    {
        settingsPanel.SetActive(true);  //открывает панель с настройками
    }
}
