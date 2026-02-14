using System;
using UnityEngine;

public interface IDamageble 
{
    //Число урона, вектор откуда пришел урон
    void TakeDamage(int amout, Vector2 hitDirection);
}
