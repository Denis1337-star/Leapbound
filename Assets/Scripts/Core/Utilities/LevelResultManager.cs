using UnityEngine;
public sealed class LevelResultManager 
{
    private const string LastLevelKey = "LastUnlockedLevel";
    private const string StarsKey = "Stars_";

    public  void SaveLevelResult(string levelName, int levelIndex, int stars)
    {
        string key = StarsKey + levelName;
        int oldStars = PlayerPrefs.GetInt(key, 0);

        if (stars > oldStars)
            PlayerPrefs.SetInt(key, stars);

        int lastUnlocked = PlayerPrefs.GetInt(LastLevelKey, 1);
        if (levelIndex + 1 > lastUnlocked)
            PlayerPrefs.SetInt(LastLevelKey, levelIndex + 1);

        PlayerPrefs.Save();
    }

    public  int GetStarsForLevel(string levelName)
    {
        return PlayerPrefs.GetInt(StarsKey + levelName, 0);
    }

    public  bool IsLevelUnlocked(int levelIndex)
    {
        int lastUnlocked = PlayerPrefs.GetInt(LastLevelKey, 1);
        return levelIndex <= lastUnlocked;
    }

    public  int GetLastUnlockedLevel()
    {
        return PlayerPrefs.GetInt(LastLevelKey, 1);
    }

    public  void ResetProgress()
    {
        PlayerPrefs.SetInt(LastLevelKey,1);
        PlayerPrefs.Save();
    }
}
