using System;

public interface IScoreService 
{
    int Score { get; }
    GameState State { get; }

    event Action<int> OnScoreChanged;
    event Action<GameState> OnGameStateChanged;

    void Add(int amount);
    void Reset();
    void Win();
    void Lose();
    void SetPaused(bool paused);
}
