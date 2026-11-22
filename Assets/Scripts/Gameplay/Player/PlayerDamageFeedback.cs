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
    private readonly ContactPoint2D[] _contacts = new ContactPoint2D[8];
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
        Vector2 knockback = hitDirection;

        if (knockback.sqrMagnitude < 0.001f)
            knockback = Vector2.up;

        knockback.y = Mathf.Max(knockback.y, 0.5f);
        knockback.Normalize();

        if (IsBlockedByWall(knockback.x))
            knockback.x = 0f;


        if (Mathf.Abs(knockback.x) < 0.01f)
            knockback = Vector2.up;

        _player.Rigidbody.AddForce(knockback * _playerConfig.KnockbackForce, ForceMode2D.Impulse);
        _knockbackTimer = _playerConfig.KnockbackLockTime;
        _invincibleTimer = _playerConfig.InvincibleTime;
        _player.Animator.SetTrigger(HurtHash);
    }
    private bool IsBlockedByWall(float knockbackX)
    {
        if (Mathf.Abs(knockbackX) < 0.01f)
            return false;

        int count = _player.Rigidbody.GetContacts(_contacts);
        for (int i = 0; i < count; i++)
        {
            Vector2 normal = _contacts[i].normal;
            if (normal.y > 0.5f || normal.y < -0.5f)
                continue;
   
            if (knockbackX > 0f && normal.x < -0.5f)
                return true;

            if (knockbackX < 0f && normal.x > 0.5f)
                return true;
        }

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(_playerConfig.GroundLayer);
        filter.useTriggers = false;

        RaycastHit2D[] hits = new RaycastHit2D[1];
        Vector2 dir = new Vector2(Mathf.Sign(knockbackX), 0f);
        int castCount = _player.BodyCollider.Cast(dir, filter, hits, 0.2f);
        return castCount > 0;
    }
    public void OnDeath()
    {
        _knockbackTimer = 0f;
        _invincibleTimer = 0f;
        _playerHealth.SetInvincible(false);
        _player.Animator.ResetTrigger(HurtHash);
    }

}
