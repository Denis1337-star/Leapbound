using System;
using Zenject;

public sealed class LevelTimer : ITickable
{
    private readonly LevelRunContext _levelRunContext;
    private readonly IGameStateService _gameStateService;
    private  float _targetSeconds;
    private bool _bonusTimeGranted;
    private float _elapsedSeconds;
    private bool _isRunning;

    public float ElapsedSeconds => _elapsedSeconds;
    public float TargetSeconds => _targetSeconds;
    public bool HasBonusTime => _bonusTimeGranted;
    public bool IsTimeTrial => _levelRunContext.Mode == LevelRunMode.TimeTrial;
    public event Action<float, float> OnTimeChanged;

    public LevelTimer(LevelRunContext levelRunContext, IGameStateService gameStateService)
    {
        _levelRunContext = levelRunContext;
        _gameStateService = gameStateService;
        _targetSeconds = levelRunContext.TargetTimeSeconds;
        _isRunning = false;
    }

    public void Tick()
    {
        if (!_isRunning)
            return;

        if (_gameStateService.CurrentState != GameState.Playing)
            return;

        _elapsedSeconds += UnityEngine.Time.deltaTime;
        OnTimeChanged?.Invoke(_elapsedSeconds, _targetSeconds);
    }

    public void Stop()
    {
        _isRunning = false;
    }
    public void StartTimer()
    {
        if (!IsTimeTrial)
            return;

        _isRunning = true;
    }

    public void ExtendTarget(float extraSeconds)
    {
        if (_bonusTimeGranted || extraSeconds <= 0f)
            return;

        _bonusTimeGranted = true;
        _targetSeconds += extraSeconds;
        OnTimeChanged?.Invoke(_elapsedSeconds, _targetSeconds);
    }
}