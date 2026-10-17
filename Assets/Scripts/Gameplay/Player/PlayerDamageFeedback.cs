using System;
using UnityEngine;
using Zenject;

public sealed class PlayerDamageFeedback : IInitializable, ITickable, IDisposable
{
    private static readonly int HurtHash = Animator.StringToHash("Hurt");
    private readonly Player _player;
    private readonly PlayerConfig _playerConfig;
    private readonly PlayerHealth _playerHealth;
    private float _invincibleTimer;
    private float _knockbackTimer;
    public bool IsInKnockback => _knockbackTimer > 0f;
    public PlayerDamageFeedback(Player player, PlayerConfig playerConfig, PlayerHealth playerHealth)
    {
        _player = player;
        _playerConfig = playerConfig;
        _playerHealth = playerHealth;
    }
    public void Initialize()
    {
        _playerHealth.OnDamaged += OnDamaged;
        _playerHealth.OnDeath += OnDeath;
    }
    public void Dispose()
    {
        _playerHealth.OnDamaged -= OnDamaged;
        _playerHealth.OnDeath -= OnDeath;
    }
    public void Tick()
    {
        if (_knockbackTimer > 0f)
            _knockbackTimer -= Time.deltaTime;

        if (_invincibleTimer <= 0f)
            return;

        _invincibleTimer -= Time.deltaTime;

        if (_invincibleTimer > 0f)
            return;

        _playerHealth.SetInvincible(false);
    }
    private void OnDamaged(Vector2 hitDirection)
    {
        _playerHealth.SetInvincible(true);
        _player.Rigidbody.linearVelocity = Vector2.zero;
        _player.Rigidbody.AddForce(hitDirection.normalized * _playerConfig.KnockbackForce,
            ForceMode2D.Impulse);

        _knockbackTimer = _playerConfig.KnockbackLockTime;
        _invincibleTimer = _playerConfig.InvincibleTime;

        _player.Animator.SetTrigger(HurtHash); 
    }
    private void OnDeath()
    {
        _knockbackTimer = 0f;
        _invincibleTimer = 0f;
        _playerHealth.SetInvincible(false);
    }

}
