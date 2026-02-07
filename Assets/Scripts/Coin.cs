using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value;  //значение очков для 1 монеты

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //Если косается с игроком 
        {
            PlayerScore.AddPoints(value); //увеличивает общий счет
            Destroy(gameObject);  //удаляется   нере
        }
    }
}
