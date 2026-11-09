using UnityEngine;
using Zenject;

public sealed class HealthPickup : MonoBehaviour
{
    [SerializeField] private AudioClip _collectClip;
    private int _healAmount = 1;
    private IAudioService _audioService;
    private HealthOneShot _healthOneShot;
    private bool _isCollected;

    [Inject]
    private void Construct(HealthOneShot healthOneShot, IAudioService audioService)
    {
        _healthOneShot = healthOneShot;
        _audioService = audioService;
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
        _audioService.PlaySFX(_collectClip);
        gameObject.SetActive(false);
    }
}
