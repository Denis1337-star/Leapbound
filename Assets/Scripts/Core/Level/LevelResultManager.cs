using YG;
public sealed class LevelResultManager
{
    private const int StarsArraySize = 16;
    public void SaveLevelResult(int levelIndex, int stars)
    {
        YG2.saves.winCount++;
        int[] starsByLevel = EnsureArray(ref YG2.saves.starsByLevelBuildIndex);
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
        int[] starsByLevel = EnsureArray(ref YG2.saves.starsByLevelBuildIndex);
        for (int i = 0; i < starsByLevel.Length; i++)
            starsByLevel[i] = 0;

        bool[] crowns = EnsureArray(ref YG2.saves.crownByLevelBuildIndex);
        float[] times = EnsureArray(ref YG2.saves.bestTimeByLevelBuildIndex);
        for (int i = 0; i < StarsArraySize; i++)
        {
            crowns[i] = false;
            times[i] = 0f;
        }

        YG2.saves.winCount = 0;
        YG2.SaveProgress();
    }
    public int GetStarsForLevel(int levelBuildIndex)
    {
        int[] starsByLevel = EnsureArray(ref YG2.saves.starsByLevelBuildIndex);
        if (levelBuildIndex < 0 || levelBuildIndex >= starsByLevel.Length)
            return 0;

        return starsByLevel[levelBuildIndex];
    }

    public bool HasCrown(int levelBuildIndex)
    {
        bool[] crowns = EnsureArray(ref YG2.saves.crownByLevelBuildIndex);
        if (levelBuildIndex < 0 || levelBuildIndex >= crowns.Length)
            return false;
        return crowns[levelBuildIndex];
    }

    public bool CanStartTimeTrial(int levelBuildIndex)
{
        return GetStarsForLevel(levelBuildIndex) >= 3;
    }

    public float GetBestTime(int levelBuildIndex)
    {
        float[] times = EnsureArray(ref YG2.saves.bestTimeByLevelBuildIndex);
        if (levelBuildIndex < 0 || levelBuildIndex >= times.Length)
            return 0f;
        return times[levelBuildIndex];
    }

    public bool HasBestTime(int levelBuildIndex)
    {
        return GetBestTime(levelBuildIndex) > 0f;
    }

    public void SaveTimeTrialResult(int levelBuildIndex, float timeSeconds, float targetSeconds)
    {
        float[] times = EnsureArray(ref YG2.saves.bestTimeByLevelBuildIndex);
        if (levelBuildIndex < 0 || levelBuildIndex >= times.Length)
            return;

        float best = times[levelBuildIndex];
        if (best <= 0f || timeSeconds < best)
            times[levelBuildIndex] = timeSeconds;

        if (timeSeconds <= targetSeconds)
        {
            bool[] crowns = EnsureArray(ref YG2.saves.crownByLevelBuildIndex);
            crowns[levelBuildIndex] = true;
        }

        YG2.SaveProgress();
    }

    private static T[] EnsureArray<T>(ref T[] field)
    {
        if (field == null || field.Length != StarsArraySize)
            field = new T[StarsArraySize];
        return field;
    }
}
