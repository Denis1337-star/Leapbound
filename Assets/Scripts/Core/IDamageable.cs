using UnityEngine;

public interface IDamageable
{
    //Число урона, вектор откуда пришел урон
    void TakeDamage(int amout, Vector2 hitDirection);
}
