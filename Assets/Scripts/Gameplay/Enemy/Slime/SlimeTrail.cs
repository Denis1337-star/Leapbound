using UnityEngine;

public sealed class SlimeTrail : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 2.5f;
    [SerializeField] private int _damage = 1;

    private float _lifeTimer;
    private bool _isBusy;
    public bool IsBusy => _isBusy;

    public void Play(Vector3 worldPosition)
    {
        transform.position = worldPosition;
        _lifeTimer = _lifeTime;
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
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_isBusy)
            return;

        if (!other.TryGetComponent(out Player player))
            return;

        if (player.PlayerHealth.CurrentHealth <= 0)
            return;

        Vector2 hitDirection = (player.transform.position - transform.position).normalized;
        player.PlayerHealth.TakeDamage(_damage, hitDirection);
    }
    public void Stop()
    {
        _isBusy = false;
        gameObject.SetActive(false);
    }
}