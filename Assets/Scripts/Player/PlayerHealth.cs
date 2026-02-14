using UnityEngine;
using System;
using Unity.VisualScripting;

public class PlayerHealth : MonoBehaviour, IDamageble
{
    public int maxHealth = 100;
    //public float invincibleTime = 1f;
    public int CurrentHP { get; private set; } //Текущее количество здоровья
    public bool IsInvincible { get; private set; } //Флаг неуязвимости

    //Событие об изменение здоровья   Передает текущее хп и maxHP
    public event Action<int, int> OnHealthChange;
    public event Action<Vector2> OnDamaged;  //событие получение урона
    public event Action OnDeath;      //Событие о смерти игрока
    private void Awake()
    {
        CurrentHP = maxHealth; //Устанавливет начальное здоровье 
        OnHealthChange?.Invoke(CurrentHP, maxHealth);  //Сигнал что здоровье изменилось
    }

    //Для получения урона
    public void TakeDamage(int amount, Vector2 hitDirection)
    {
        if (IsInvincible || CurrentHP <= 0)  //если неуязвим или мертв = выход
        {
            return;
        }

        //Уменьшаем текущее хп на величину урона
        CurrentHP -= amount;

        //Ограничиваем значения текущего хп от 0 до maxHP
        CurrentHP = Mathf.Clamp(CurrentHP, 0, maxHealth);

        OnHealthChange?.Invoke(CurrentHP, maxHealth);   //Сигнал об изменение хп
        OnDamaged?.Invoke(hitDirection);  //Сигнал об получение урона (передает направление от куда)

        //Проверка здоровья и сигнал смерти
        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        CurrentHP = Mathf.Min(CurrentHP + amount, maxHealth);  //увеличиваем ХП но не больше MAX
        OnHealthChange?.Invoke(CurrentHP, maxHealth);  //сигнал
    }

    public void Kill()
    {
        CurrentHP = 0;  
        OnHealthChange?.Invoke(0, maxHealth);
        Die();
    }

    public void Die()
    {
        OnDeath?.Invoke();  //всем системам сигнал
        GetComponent<PlayerDeathEffect>().Play();  //запускаем эффект смерти
    }
    public void SetInvincible(bool value)
    {
        IsInvincible = value;  
    }
}
