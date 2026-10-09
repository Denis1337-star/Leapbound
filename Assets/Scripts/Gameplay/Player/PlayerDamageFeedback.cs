using System;
using UnityEngine;
using Zenject;



public sealed class PlayerDamageFeedback : IInitializable, ITickable, IDisposable
{
    private readonly Player _player;
    private readonly PlayerConfig _playerConfig;
    private readonly PlayerHealth _playerHealth;
    private bool _isBlinking;
    private float _invincibleTimer;
    private float _blinkTimer;
    private bool _spriteVisible = true;
    public PlayerDamageFeedback(Player player, PlayerConfig playerConfig, PlayerHealth playerHealth)
    {
        _player = player;
        _playerConfig = playerConfig;
        _playerHealth = playerHealth;
    }
    public void Initialize()
    {
        _playerHealth.OnDamaged += OnDamaged;
        _playerHealth.OnDeath += StopBlink;
    }
    public void Dispose()
    {
        _playerHealth.OnDamaged -= OnDamaged;
        _playerHealth.OnDeath -= StopBlink;
    }
    public void Tick()
    {
        if (!_isBlinking)
            return;

        _invincibleTimer -= Time.deltaTime;
        _blinkTimer -= Time.deltaTime;

        if (_blinkTimer <= 0f)
        {
            _spriteVisible = !_spriteVisible;
            _player.SpriteRenderer.enabled = _spriteVisible;
            _blinkTimer = 0.08f;
        }

        if (_invincibleTimer > 0f)
            return;

        _isBlinking = false;
        _player.SpriteRenderer.enabled = true;
        _player.SpriteRenderer.color = _playerConfig.NormalColor;
        _playerHealth.SetInvincible(false);
    }
    private void OnDamaged(Vector2 hitDirection)
    {
        _playerHealth.SetInvincible(true);
        _player.Rigidbody.linearVelocity = Vector2.zero;
        _player.Rigidbody.AddForce(hitDirection.normalized * _playerConfig.KnockbackForce,
            ForceMode2D.Impulse);

        _player.SpriteRenderer.color = _playerConfig.DamageColor;
        _isBlinking = true;
        _invincibleTimer = _playerConfig.InvincibleTime;
        _blinkTimer = 0.08f;
        _spriteVisible = false;
        _player.SpriteRenderer.enabled = false;
    }
    private void StopBlink()
    {
        _isBlinking = false;
        _player.SpriteRenderer.enabled = true;
    }

}
