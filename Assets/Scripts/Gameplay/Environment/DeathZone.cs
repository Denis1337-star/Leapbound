using UnityEngine;

public sealed class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.TryGetComponent(out Player player))
            return;

        player.PlayerHealth.Kill();
    }
}
