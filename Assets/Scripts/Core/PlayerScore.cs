using System;

public static class PlayerScore
{
    public static int Score { get; private set; }

    public static bool IsGameOver { get; private set; }
    public static bool IsWin { get; private set; }

    public static event Action<int> OnScoreChanged;

    public static void Add(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }

    public static void Reset()
    {
        Score = 0;
        IsGameOver = false;
        IsWin = false;
    }

    public static void Win()
    {
        IsGameOver = true;
        IsWin = true;
    }

    public static void Lose()
    {
        IsGameOver = true;
        IsWin = false;
    }
}
