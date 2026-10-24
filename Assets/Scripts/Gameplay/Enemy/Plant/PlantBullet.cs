using UnityEngine;

public sealed class PlantBullet : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private int _damage = 50;
    [SerializeField] private LayerMask _stopLayer;
    [SerializeField] private Rigidbody2D _rigidbody;
    private BulletBreakEffectPool _bulletBreakEffectPool;

    private Vector2 _direction;
    private float _lifeTimer;
    private Vector2 _homePosition;

    public void SetBreakEffectPool(BulletBreakEffectPool bulletBreakEffectPool)
    {
        _bulletBreakEffectPool = bulletBreakEffectPool;
    }
    public void Launch(Vector2 direction)
    {
        _direction = direction.normalized;
        _lifeTimer = _lifeTime;
        gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        _lifeTimer -= Time.fixedDeltaTime;
        if (_lifeTimer <= 0f)
        {
            ReturnToPool();
            return;
        }

        Vector2 nextPosition = _rigidbody.position + _direction * _speed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(nextPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Vector2 hitDir = (player.transform.position - transform.position).normalized;
            player.PlayerHealth.TakeDamage(_damage, hitDir);
            Break();
            return;
        }

        if (IsStopLayer(other.gameObject.layer))
            Break();
    }
    private bool IsStopLayer(int layer)
    {
        return (_stopLayer.value & (1 << layer)) != 0;
    }
    private void ReturnToPool()
    {
        if (!gameObject.activeSelf)
            return;

        _direction = Vector2.zero;
        _rigidbody.linearVelocity = Vector2.zero;
        transform.localPosition = Vector3.zero;
        _rigidbody.position = transform.position;
        gameObject.SetActive(false);
    }
    private void Break()
    {
        _bulletBreakEffectPool.Play(transform.position);
        ReturnToPool();
    }
}
