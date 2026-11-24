using DG.Tweening;
using YG;

public sealed class LevelResultManager
{
    private const int StarsArraySize = 16;

    public void SaveLevelResult(int levelIndex, int stars)
    {
        YG2.saves.winCount++;
        int[] starsByLevel = EnsureArray(ref YG2.saves.starsByLevelBuildIndex);

        int oldStars = 0;
        bool improvedStars = false;
        if (levelIndex >= 0 && levelIndex < starsByLevel.Length)
        {
            oldStars = starsByLevel[levelIndex];
            if (stars > oldStars)
            {
                starsByLevel[levelIndex] = stars;
                improvedStars = true;
            }
        }

        if (levelIndex + 1 > YG2.saves.lastUnlockedLevelBuildIndex)
            YG2.saves.lastUnlockedLevelBuildIndex = levelIndex + 1;

        if (improvedStars)
            YG2.SetLeaderboard(LeaderboardIds.Stars, SumStars(starsByLevel));

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
        bool improvedTime = best <= 0f || timeSeconds < best;
        if (improvedTime)
        {
            times[levelBuildIndex] = timeSeconds;
            YG2.SetLBTimeConvert(LeaderboardIds.Time(levelBuildIndex), timeSeconds);
        }

        bool[] crowns = EnsureArray(ref YG2.saves.crownByLevelBuildIndex);
        bool hadCrown = crowns[levelBuildIndex];
        if (timeSeconds <= targetSeconds)
            crowns[levelBuildIndex] = true;

        bool newCrown = !hadCrown && crowns[levelBuildIndex];
        if (newCrown)
        {
            int crownCount = CountCrowns(crowns);
            if (improvedTime)
            {
                DOVirtual.DelayedCall(1.1f, () =>
                {
                    YG2.SetLeaderboard(LeaderboardIds.Crowns, crownCount);
                }).SetUpdate(true);
            }
            else
            {
                YG2.SetLeaderboard(LeaderboardIds.Crowns, crownCount);
            }
        }

        YG2.SaveProgress();
    }

    private static int SumStars(int[] starsByLevel)
    {
        int sum = 0;
        for (int i = 0; i < starsByLevel.Length; i++)
            sum += starsByLevel[i];

        return sum;
    }

    private static int CountCrowns(bool[] crowns)
    {
        int count = 0;
        for (int i = 0; i < crowns.Length; i++)
        {
            if (crowns[i])
                count++;
        }
        return count;
    }

    private static T[] EnsureArray<T>(ref T[] field)
    {
        if (field == null || field.Length != StarsArraySize)
            field = new T[StarsArraySize];

        return field;
    }
}