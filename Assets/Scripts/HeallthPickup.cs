using UnityEngine;

public class HeallthPickup : MonoBehaviour
{
    public int healAmount = 100;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        var health = col.GetComponent<PlayerHealth>();
        if (health != null)
            health.Heal(healAmount);

        Destroy(gameObject);
    }
}
