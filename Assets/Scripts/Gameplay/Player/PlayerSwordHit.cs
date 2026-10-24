using UnityEngine;

public sealed class PlayerSwordHit : MonoBehaviour
{
    [SerializeField] private Collider2D _hitbox;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private readonly Collider2D[] _overlap = new Collider2D[8];
    private readonly EnemyBase[] _alreadyHit = new EnemyBase[8];
    private int _hitCount;
    private float _offsetX;
    private void Awake()
    {
        _offsetX = Mathf.Abs(_hitbox.transform.localPosition.x);
        _hitbox.enabled = false;
    }
    private void LateUpdate()
    {
        Vector3 position = _hitbox.transform.localPosition;
        if (_spriteRenderer.flipX)
            position.x = -_offsetX;
        else
            position.x = _offsetX;

        _hitbox.transform.localPosition = position;
    }
    public void Begin()
    {
        _hitCount = 0;
        _hitbox.enabled = true;
        Collect();
    }
    public void End()
    {
        _hitbox.enabled = false;
        _hitCount = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryHit(collision);
    }
    private void Collect()
    {
        ContactFilter2D filter = ContactFilter2D.noFilter;
        int count = _hitbox.Overlap(filter, _overlap);
        for (int i = 0; i < count; i++)
        {
            TryHit(_overlap[i]);
        }

    }
    private void TryHit(Collider2D other)
    {
        if (!_hitbox.enabled)
            return;

        if (other.TryGetComponent(out PlantBullet bullet))
        {
            bullet.Break();
            return;
        }

        if (!other.TryGetComponent(out EnemyBase enemy))
            return;

        for (int i = 0; i < _hitCount; i++)
        {
            if (_alreadyHit[i] == enemy)
                return;
        }

        if (_hitCount >= _alreadyHit.Length)
            return;

        _alreadyHit[_hitCount] = enemy;
        _hitCount++;

        Vector2 direction = (enemy.transform.position - transform.position).normalized;
        enemy.Kill(direction);
    }
}
