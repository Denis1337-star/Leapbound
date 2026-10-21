using UnityEngine;
using Zenject;

public sealed class HealthPickup : MonoBehaviour
{
    [SerializeField] private int _healAmount = 100;
    private HealthOneShot _healthOneShot;
    private bool _isCollected;

    [Inject]
    private void Construct(HealthOneShot healthOneShot)
    {
        _healthOneShot = healthOneShot;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (_isCollected)
            return;

        if (!collider.TryGetComponent(out Player player))
            return;

        _isCollected = true;
        player.PlayerHealth.Heal(_healAmount);
        _healthOneShot.Play();
        gameObject.SetActive(false);
    }
}
