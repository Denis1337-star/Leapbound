using UnityEngine;
using System;


public abstract class EnemyBase : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int _maxHealth = 50;
    public int CurrentHP { get; private set; }

    public event Action<Vector2> OnDamaged;
    public event Action OnDeath;
    public int MaxHealth => _maxHealth;

    protected virtual void Awake()
    {
        CurrentHP = _maxHealth;
    }

    public virtual void TakeDamage(int amount, Vector2 hitDir)
    {
        if (CurrentHP <= 0)
            return;

        CurrentHP -= amount;
        CurrentHP = Mathf.Max(CurrentHP, 0);

        OnDamaged?.Invoke(hitDir);

        if (CurrentHP <= 0)
            Die();
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke();
    }
}
