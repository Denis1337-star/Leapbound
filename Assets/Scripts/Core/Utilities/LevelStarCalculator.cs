public static class LevelStarCalculator 
{
    public const int ThreeStarsThreshold = 100;
    public const int TwoStarsThreshold = 50;

    public static int Calculate(int score)
    {
        if (score >= ThreeStarsThreshold) return 3;
        if(score >= TwoStarsThreshold) return 2;
        return 1;
    }
}
