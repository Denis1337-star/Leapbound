

public readonly struct LevelStarResult
{
    public readonly bool FinishMet;
    public readonly bool ScoreMet;
    public readonly bool NoDeathMet;
    public readonly int Count;

    public LevelStarResult(bool finishMet, bool scoreMet, bool noDeathMet)
    {
        FinishMet = finishMet;
        ScoreMet = scoreMet;
        NoDeathMet = noDeathMet;
        Count = 0;

        if (finishMet)
            Count++;

        if (scoreMet)
            Count++;

        if (noDeathMet)
            Count++;
    }
}
