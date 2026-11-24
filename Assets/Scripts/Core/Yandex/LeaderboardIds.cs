
public static class LeaderboardIds
{
    public static string Time(int levelBuildIndex) =>
        string.Format("LeaderboardLevelTime{0}", levelBuildIndex);

    public const string Crowns = "LeaderboardCrowns";
    public const string Stars = "LeaderboardStars";
}