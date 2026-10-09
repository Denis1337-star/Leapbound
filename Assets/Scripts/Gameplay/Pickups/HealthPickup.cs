using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int _healAmount = 100;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        if (collider.TryGetComponent(out Player player))
            player.PlayerHealth.Heal(_healAmount);

        gameObject.SetActive(false);
    }
}
