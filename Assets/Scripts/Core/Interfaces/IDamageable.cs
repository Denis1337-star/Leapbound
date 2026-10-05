using System;
using UnityEngine;

public interface IDamageable
{
    int CurrentHP { get; }
    int MaxHP { get; }
    event Action OnDied;
    event Action<int, int> OnHealthChanged;
    void TakeDamage(int damageAmount, Vector2 hitPoint);
    void Heal(int healAmount);
}
