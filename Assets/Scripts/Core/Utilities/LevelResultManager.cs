using UnityEngine;
public static class LevelResultManager 
{
    private const string LAST_LEVEL_KEY = "LastUnlockedLevel";
    private const string STARS_KEY = "Stars_";

    public static void SaveLevelResult(string levelName, int levelIndex, int stars)
    {
        string key = STARS_KEY + levelName;

        int oldStars = PlayerPrefs.GetInt(key, 0);

        if (stars > oldStars)
            PlayerPrefs.SetInt(key, stars);

        int lastUnlocked = PlayerPrefs.GetInt(LAST_LEVEL_KEY, 1);

        if (levelIndex + 1 > lastUnlocked)
            PlayerPrefs.SetInt(LAST_LEVEL_KEY, levelIndex + 1);

        PlayerPrefs.Save();
    }

    public static int GetStarsForLevel(string levelName)
    {
        return PlayerPrefs.GetInt(STARS_KEY + levelName, 0);
    }

    public static bool IsLevelUnlocked(int levelIndex)
    {
        int lastUnlocked = PlayerPrefs.GetInt(LAST_LEVEL_KEY, 1);
        return levelIndex <= lastUnlocked;
    }

    public static int GetLastUnlockedLevel()
    {
        return PlayerPrefs.GetInt(LAST_LEVEL_KEY, 1);
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
