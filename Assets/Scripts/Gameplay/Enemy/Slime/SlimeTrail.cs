using UnityEngine;

public sealed class SlimeTrail : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 2.5f;
    [SerializeField] private int _damage = 1;

    private float _lifeTimer;
    private bool _isBusy;
    private bool _hitPlayer;

    public bool IsBusy => _isBusy;

    public void Play(Vector3 worldPosition)
    {
        transform.position = worldPosition;
        _lifeTimer = _lifeTime;
        _hitPlayer = false;
        _isBusy = true;
        gameObject.SetActive(true);
    }
    private void Update()
    {
        if (!_isBusy)
            return;

        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer > 0f)
            return;

        Stop();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isBusy || _hitPlayer)
            return;

        if (!other.TryGetComponent(out Player player))
            return;

        if (player.PlayerHealth.CurrentHealth <= 0)
            return;

        Vector2 hitDirection = (player.transform.position - transform.position).normalized;
        player.PlayerHealth.TakeDamage(_damage, hitDirection);
        _hitPlayer = true;
    }
    public void Stop()
    {
        _isBusy = false;
        gameObject.SetActive(false);
    }
}