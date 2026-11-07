using System;
using UnityEngine;
using Zenject;

public sealed class PlayerDeathEffect : IInitializable, ITickable, IDisposable
{
    private readonly Player _player;
    private readonly PlayerHealth _playerHealth;
    private readonly PlayerRespawn _playerRespawn;
    private readonly PlayerAnimation _playerAnimation;
    private readonly PlayerLives _playerLives;
    private readonly IGameStateService _gameStateService;
    private float _restartTimer = -1f;

    public PlayerDeathEffect(Player player, PlayerHealth playerHealth, PlayerRespawn playerRespawn,
        PlayerAnimation playerAnimation, PlayerLives playerLives, IGameStateService gameStateService)
    {
        _player = player;
        _playerHealth = playerHealth;
        _playerRespawn = playerRespawn;
        _playerAnimation = playerAnimation;
        _playerLives = playerLives;
        _gameStateService = gameStateService;
    }
    public void Initialize() { _playerHealth.OnDeath += OnDeath; }
    public void Dispose() { _playerHealth.OnDeath -= OnDeath; }
    public void Tick()
    {
        if (_restartTimer < 0f)
            return;

        _restartTimer -= Time.deltaTime;
        if (_restartTimer > 0f)
            return;

        _restartTimer = -1f;
        HandleDeathFinished();
    }
    private void OnDeath()
    {
        _player.SpriteRenderer.enabled = true;
        _playerAnimation.SetUnscaled(true);
        _player.FreezeBody();
        _restartTimer = 1.6f;
    }
    private void HandleDeathFinished()
    {
        _playerLives.LoseLife();

        if (_playerLives.CurrentLives > 0)
        {
            _playerRespawn.Respawn();
            return;
        }

        _gameStateService.Lose();
    }
}