using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 10; // очки за монету

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerScore.Add(value);  // увеличиваем очки
        Destroy(gameObject);     // удаляем монету
    }
}
