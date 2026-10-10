using UnityEngine;

public sealed class HealthPickup : MonoBehaviour
{
    [SerializeField] private int _healAmount = 100;
    private bool _isCollected;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (_isCollected)
            return;

        if (!collider.TryGetComponent(out Player player))
            return;

        _isCollected = true;
        player.PlayerHealth.Heal(_healAmount);
        gameObject.SetActive(false);
    }
}
