using UnityEngine;
using Zenject;

public sealed class HitStopPulse : MonoBehaviour
{
    [SerializeField] private float _duration = 0.055f;
    [SerializeField] private float _frozenTimeScale = 0.05f;

    private IGameStateService _gameStateService;
    private float _timer = -1f;

    [Inject]
    private void Construct(IGameStateService gameStateService)
    {
        _gameStateService = gameStateService;
    }
    private void Update()
    {
        if (_timer < 0f)
            return;

        _timer -= Time.unscaledDeltaTime;
        if (_timer > 0f)
            return;

        _timer = -1f;
        RestoreTimeScale();
    }
    public void Play()
    {
        if (_gameStateService.CurrentState != GameState.Playing)
            return;

        Time.timeScale = _frozenTimeScale;
        _timer = _duration;
    }
    private void RestoreTimeScale()
    {
        if (_gameStateService.CurrentState != GameState.Playing)
            return;

        Time.timeScale = 1f;
    }
    private void OnDisable()
    {
        if (_timer < 0f)
            return;

        _timer = -1f;
        if (_gameStateService.CurrentState == GameState.Playing)
            Time.timeScale = 1f;
    }
}