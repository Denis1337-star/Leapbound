using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelResultManager : MonoBehaviour
{
    public static LevelResultManager Instance { get; private set; }

    private const string LAST_LEVEL_KEY = "LastUnlockedLevel";
    private const string STARS_KEY = "Stars_";

    //СОХРАНЕНИЕ РЕЗУЛЬТАТА 
    public static void SaveLevelResult(string levelName, int levelIndex, int stars)
    {
        string key = STARS_KEY + levelName;

        int oldStars = PlayerPrefs.GetInt(key, 0);

        // сохраняем ТОЛЬКО лучший результат
        if (stars > oldStars)
            PlayerPrefs.SetInt(key, stars);

        int lastUnlocked = PlayerPrefs.GetInt(LAST_LEVEL_KEY, 1);

        // открываем следующий уровень
        if (levelIndex + 1 > lastUnlocked)
            PlayerPrefs.SetInt(LAST_LEVEL_KEY, levelIndex + 1);

        PlayerPrefs.Save();
    }

    // ЗВЕЗДЫ
    public static int GetStarsForLevel(string levelName)
    {
        return PlayerPrefs.GetInt(STARS_KEY + levelName, 0);
    }

    //ДОСТУП К УРОВНЮ
    public static bool IsLevelUnlocked(int levelIndex)
    {
        int lastUnlocked = PlayerPrefs.GetInt(LAST_LEVEL_KEY, 1);
        return levelIndex <= lastUnlocked;
    }

    public static int GetLastUnlockedLevel()
    {
        return PlayerPrefs.GetInt(LAST_LEVEL_KEY, 1);
    }

    // (опционально)
    public static void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
