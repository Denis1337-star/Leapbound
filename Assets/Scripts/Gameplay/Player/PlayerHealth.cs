using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
   [SerializeField] private int _maxHealth = 100;
    public int CurrentHP { get; private set; } 
    public bool IsInvincible { get; private set; } 
    public event Action<int, int> OnHealthChanged;
    public event Action<Vector2> OnDamaged; 
    public event Action OnDeath;     
    private bool _isDead;
    public int MaxHealth => _maxHealth;
    private void Awake()
    {
        CurrentHP = _maxHealth;
        OnHealthChanged?.Invoke(CurrentHP, _maxHealth);  
    }


    public void TakeDamage(int amount, Vector2 hitDirection)
    {
        if (IsInvincible || CurrentHP <= 0) return; 
   
        CurrentHP -= amount;

        CurrentHP = Mathf.Clamp(CurrentHP, 0, _maxHealth);

        OnHealthChanged?.Invoke(CurrentHP, _maxHealth);  
        OnDamaged?.Invoke(hitDirection);  

        if (CurrentHP <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        CurrentHP = Mathf.Min(CurrentHP + amount, _maxHealth);
        OnHealthChanged?.Invoke(CurrentHP, _maxHealth); 
    }

    public void Kill()
    {
        CurrentHP = 0;
        OnHealthChanged?.Invoke(0, _maxHealth);
        Die();
    }

    public void Die()
    {
        if(_isDead) return;
        _isDead = true;
        OnDeath?.Invoke();  
    }
    public void SetInvincible(bool value)
    {
        IsInvincible = value;  
    }
}
