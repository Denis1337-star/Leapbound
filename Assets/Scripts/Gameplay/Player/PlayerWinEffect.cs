using UnityEngine;
using Zenject;

public sealed class PlayerWinEffect : ITickable
{
    private readonly PlayerAnimation _playerAnimation;
    private readonly IGameStateService _gameStateService;
    private float _panelTimer = -1f;

    public PlayerWinEffect(PlayerAnimation playerAnimation, IGameStateService gameStateService)
    {
        _playerAnimation = playerAnimation;
        _gameStateService = gameStateService;
    }
    public void Begin()
    {
        _playerAnimation.PlayWin();
        _playerAnimation.SetUnscaled(true);
        _panelTimer = 1f;
    }
    public void Tick()
    {
        if (_panelTimer < 0f)
            return;

        _panelTimer -= Time.deltaTime;
        if (_panelTimer > 0f)
            return;

        _panelTimer = -1f;
        _gameStateService.Win();
    }
}