using System;

public sealed class GameStateService : IGameStateService
{
    public GameState CurrentState { get; private set; } = GameState.Playing;
    public event Action<GameState> OnStateChanged;

    public void SetPaused(bool paused)
    {
        if (paused)
            ChangeState(GameState.Paused);
        else
            ChangeState(GameState.Playing);
    }
    public void Win()
    {
        ChangeState(GameState.Win);
    }
    public void Lose()
    {
        ChangeState(GameState.Lose);
    }
    public void Reset()
    {
        ChangeState(GameState.Playing);
    }
    private void ChangeState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        OnStateChanged?.Invoke(CurrentState);
    }
}
