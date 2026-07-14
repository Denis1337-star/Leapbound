using System;
public class ScoreService : IScoreService
{
    public int Score { get; private set; }
    public GameState State { get; private set; }
    public event Action<int> OnScoreChanged;
    public event Action<GameState> OnGameStateChanged;

    public void Add(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }
    public void Reset()
    {
        Score = 0;
        SetState(GameState.Playing);
        OnScoreChanged?.Invoke(Score);
    }
    public void Win()
    {
        SetState(GameState.Won);
    }
    public void Lose()
    {
        SetState(GameState.Lost);
    }
    public void SetPaused(bool paused)
    {
        if (paused)
        { SetState(GameState.Paused);}
        else { SetState(GameState.Playing); }
    }

    private void SetState(GameState newState)
    {
        if(State == newState) return;

        State = newState;
        OnGameStateChanged?.Invoke(State);
    }
}
