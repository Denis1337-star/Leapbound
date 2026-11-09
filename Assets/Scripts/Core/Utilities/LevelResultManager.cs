using YG;
public sealed class LevelResultManager
{
    private const int StarsArraySize = 16;
    public void SaveLevelResult(int levelIndex, int stars)
    {
        YG2.saves.winCount++;
        int[] starsByLevel = GetStarsArray();
        if (levelIndex >= 0 && levelIndex < starsByLevel.Length)
        {
            if (stars > starsByLevel[levelIndex])
                starsByLevel[levelIndex] = stars;
        }

        if (levelIndex + 1 > YG2.saves.lastUnlockedLevelBuildIndex)
            YG2.saves.lastUnlockedLevelBuildIndex = levelIndex + 1;

        YG2.SaveProgress();
    }
    public bool IsLevelUnlocked(int levelIndex)
    {
        return levelIndex <= YG2.saves.lastUnlockedLevelBuildIndex;
    }
    public int GetLastUnlockedLevel()
    {
        return YG2.saves.lastUnlockedLevelBuildIndex;
    }
    public void ResetProgress()
    {
        YG2.saves.lastUnlockedLevelBuildIndex = 1;
        int[] starsByLevel = GetStarsArray();
        for (int i = 0; i < starsByLevel.Length; i++)
            starsByLevel[i] = 0;

        YG2.saves.winCount = 0;
        YG2.SaveProgress();
    }
    public int GetStarsForLevel(int levelBuildIndex)
    {
        int[] starsByLevel = GetStarsArray();
        if (levelBuildIndex < 0 || levelBuildIndex >= starsByLevel.Length)
            return 0;

        return starsByLevel[levelBuildIndex];
    }
    private static int[] GetStarsArray()
    {
        if (YG2.saves.starsByLevelBuildIndex == null ||
            YG2.saves.starsByLevelBuildIndex.Length != StarsArraySize)
        {
            YG2.saves.starsByLevelBuildIndex = new int[StarsArraySize];
        }
        return YG2.saves.starsByLevelBuildIndex;
    }
}
