using System;
using UnityEngine;

public sealed class PlayerHealth
{
    private readonly int _maxHealth;
    private bool _isDead;
    public int CurrentHealth { get; private set; }
    public int MaxHealth => _maxHealth;
    public bool IsInvincible { get; private set; }
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
}
