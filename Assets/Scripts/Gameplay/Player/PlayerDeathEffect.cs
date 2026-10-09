using System;
using UnityEngine;
using Zenject;

public sealed class PlayerDeathEffect : IInitializable, ITickable, IDisposable
{
    private readonly Player _player;
    private readonly PlayerHealth _playerHealth;
    private readonly IGameStateService _gameStateService;
    private readonly ISceneFlowService _sceneFlowService;
    private float _restartTimer = -1f;
    public PlayerDeathEffect(Player player, PlayerHealth playerHealth, IGameStateService gameStateService,
        ISceneFlowService sceneFlowService)
    {
        _player = player;
        _playerHealth = playerHealth;
        _gameStateService = gameStateService;
        _sceneFlowService = sceneFlowService;
    }
    public void Initialize() { _playerHealth.OnDeath += OnDeath; }
    public void Dispose() { _playerHealth.OnDeath -= OnDeath; }
    public void Tick()
    {
        if (_restartTimer < 0f)
            return;

        _restartTimer -= Time.unscaledDeltaTime;
        if (_restartTimer > 0f)
            return;

        _restartTimer = -1f;
        _sceneFlowService.RestartCurrentLevel();
    }
    private void OnDeath()
    {
        _player.Rigidbody.linearVelocity = Vector2.zero;
        _player.SpriteRenderer.enabled = true;
        _player.Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        _gameStateService.Lose();
        _restartTimer = 1.2f;
    }
}
