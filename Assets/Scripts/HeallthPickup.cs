using UnityEngine;

public class HeallthPickup : MonoBehaviour
{
    public int healAmount = 100;   //сколько хилит 

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            var health = col.GetComponent<PlayerHealthUI>();

            if (health != null)
            {
                health.Heal(healAmount); // Вылечить
            }

            Destroy(gameObject); // Удалить объект хилки
        }
    }
}
