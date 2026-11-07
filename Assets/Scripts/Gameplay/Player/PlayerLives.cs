using System;
using UnityEngine;

public sealed class PlayerLives
{
    private readonly int _maxLives;
    public int CurrentLives { get; private set; }
    public int MaxLives => _maxLives;
    public event Action<int, int> OnLivesChanged;
    public event Action OnLivesDepleted;

    public PlayerLives(int maxLives = 3)
    {
        _maxLives = maxLives;
        CurrentLives = maxLives;
    }
    public void LoseLife()
    {
        if (CurrentLives <= 0)
            return;

        CurrentLives--;
        OnLivesChanged?.Invoke(CurrentLives, _maxLives);

        if (CurrentLives <= 0)
            OnLivesDepleted?.Invoke();
    }
    public void AddLife(int amount = 1)
    {
        CurrentLives = Mathf.Min(CurrentLives + amount, _maxLives);
        OnLivesChanged?.Invoke(CurrentLives, _maxLives);
    }
    public void ResetLives()
    {
        CurrentLives = _maxLives;
        OnLivesChanged?.Invoke(CurrentLives, _maxLives);
    }
}