using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        if (collider.TryGetComponent(out Player player))
            player.PlayerHealth.Kill();
    }
}
