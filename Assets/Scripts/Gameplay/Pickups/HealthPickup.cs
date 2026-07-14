using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int _healAmount = 100;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        var health = col.GetComponent<PlayerHealth>();
        if (health != null)
            health.Heal(_healAmount);

        Destroy(gameObject);
    }
}
