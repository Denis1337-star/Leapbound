
using System;

public interface IGameStateService 
{
    GameState CurrentState { get; }
    event Action<GameState> OnStateChanged;

    void SetPaused(bool paused);
    void Win();
    void Lose();
    void Reset();
}
