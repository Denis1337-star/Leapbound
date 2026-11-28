using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public sealed class LevelSessionStats : ITickable
{
    private readonly IGameStateService _gameStateService;
    private float _elapsedSeconds;
    private int _deaths;
    private bool _started;

    public int Deaths => _deaths;
    public int ElapsedSecondsRounded
    {
        get
        {
            int value = (int)_elapsedSeconds;
            return value;
        }
    }
    public int LevelBuildIndex => SceneManager.GetActiveScene().buildIndex;
    public bool HasStarted => _started;

    public LevelSessionStats(IGameStateService gameStateService)
    {
        _gameStateService = gameStateService;
    }

    public void Tick()
    {
        if (!_started)
            return;

        if (_gameStateService.CurrentState != GameState.Playing)
            return;

        _elapsedSeconds += Time.deltaTime;
    }

    public void MarkStarted()
    {
        _started = true;
        _elapsedSeconds = 0f;
        _deaths = 0;
    }

    public void RegisterDeath()
    {
        if (!_started)
            return;

        _deaths++;
    }
}