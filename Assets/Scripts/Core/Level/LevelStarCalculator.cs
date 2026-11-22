
public sealed class LevelStarCalculator
{
    private readonly int _targetScore;
    public int TargetScore => _targetScore;

    public LevelStarCalculator(int targetScore)
    {
        _targetScore = targetScore;
    }

    public LevelStarResult Calculate(int score, bool hasLostLife, bool finished)
    {
        bool scoreMet = score >= _targetScore;
        bool noDeathMet = !hasLostLife;
        return new LevelStarResult(finished, scoreMet, noDeathMet);
    }
}