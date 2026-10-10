public sealed class LevelStarCalculator 
{
    public readonly int _threeStarScore;
    public LevelStarCalculator(int threeStarScore)
    {
        _threeStarScore = threeStarScore;
    }

    public  int Calculate(int score)
    {
        int twoStarsScore = _threeStarScore / 2;
        if (score >= _threeStarScore)
            return 3;

        if (score >= twoStarsScore)
            return 2;

        return 1;
    }
}
