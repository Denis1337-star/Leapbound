using UnityEngine;

public class PigEnemy : EnemyBase
{
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private int _damage = 50;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Collider2D _collider;

    private Vector3 _target;

    protected override void Awake()
    {
        base.Awake();
        _target = _pointB.position;
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            _target,_speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _target) < 0.05f)
        {
            _target = _target == _pointA.position ? _pointB.position : _pointA.position;
            _sprite.flipX = !_sprite.flipX;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        float playerBottom = collision.collider.bounds.min.y;
        float enemyTop = _collider.bounds.max.y;


        if (playerBottom > enemyTop - 0.05f)
        {
            TakeDamage(MaxHealth, Vector2.up);
        }
        else if(collision.collider.TryGetComponent(out Player player))
        {
                Vector2 hitDirection = (player.transform.position - transform.position).normalized;
                player.PlayerHealth.TakeDamage(_damage, hitDirection);
        }
    }
}
