using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _value = 10; // очки за монету

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerScore.Add(_value);  // увеличиваем очки
        Destroy(gameObject);     // удаляем монету
    }
}
