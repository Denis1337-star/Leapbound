using System;
using UnityEngine;

public sealed class PlayerHealth
{
    private int _maxHealth;
    private bool _isDead;
    private bool _bonusMaxHealthGranted;
    public int CurrentHealth { get; private set; }
    public bool IsInvincible { get; private set; }
    public bool HasBonusMaxHealth => _bonusMaxHealthGranted;
    public int MaxHealth => _maxHealth;
    public event Action<int, int> OnHealthChanged;
    public event Action<Vector2> OnDamaged;
    public event Action OnDeath;
    public PlayerHealth(PlayerConfig playerConfig)
    {
        _maxHealth = playerConfig.MaxHealth;
        CurrentHealth = _maxHealth;
    }
    public void TakeDamage(int amount, Vector2 hitDirection)
    {
        if (IsInvincible || CurrentHealth <= 0)
            return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, _maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);
        OnDamaged?.Invoke(hitDirection);

        if (CurrentHealth <= 0)
            Die();
    }
    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, _maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);
    }
    public void Kill()
    {
        CurrentHealth = 0;
        OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);
        Die();
    }
    public void SetInvincible(bool value) { IsInvincible = value; }
    private void Die()
    {
        if (_isDead)
            return;

        _isDead = true;
        OnDeath?.Invoke();
    }
    public void Revive()
    {
        _isDead = false;
        IsInvincible = false;
        CurrentHealth = _maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);
    }
    public void GrantBonusMaxHealth()
    {
        if (_bonusMaxHealthGranted)
            return;

        _bonusMaxHealthGranted = true;
        _maxHealth += 1;
        CurrentHealth = _maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, _maxHealth);
    }
}
