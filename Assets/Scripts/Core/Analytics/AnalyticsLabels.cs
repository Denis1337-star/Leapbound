public static class AnalyticsLabels
{
    public static string RunMode(LevelRunContext levelRunContext)
    {
        if (levelRunContext.LevelMode == LevelRunMode.TimeTrial)
            return "time_trial";
        return "normal";
    }

    public static string SelectedHero(HeroId heroId)
    {
        if (heroId == HeroId.Rogue)
            return "Rogue";
        if (heroId == HeroId.Mage)
            return "Mage";
        return "Knight";
    }
}