using UnityEngine;
using System;


public abstract class EnemyBase : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public int maxHealth = 50;
    public int CurrentHP { get; private set; }

    public event Action<Vector2> OnDamaged;
    public event Action OnDeath;

    private void Awake()
    {
        CurrentHP = maxHealth;
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
