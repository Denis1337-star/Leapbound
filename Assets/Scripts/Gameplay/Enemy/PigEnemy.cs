using UnityEngine;

public class PigEnemy : EnemyBase
{
    [Header("Patrol")]
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private int _damage = 50;

    private Vector3 _target;
    private SpriteRenderer _sprite;
    private Collider2D _collider;

    protected override void Awake()
    {
        base.Awake();
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _target = _pointB.position;
    }
    protected override bool ValidateInternal()
    {
        bool valid = true;
        valid &= ValidationUtility.IsAssigned(this, _pointA, nameof(_pointA));
        valid &= ValidationUtility.IsAssigned(this, _pointB, nameof(_pointB));
        return valid;
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

        PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();

        if (playerBottom > enemyTop - 0.05f)
        {
            TakeDamage(maxHealth, Vector2.up);
        }
        else
        {
            if (playerHealth != null)
            {
                Vector2 hitDirection = (collision.collider.transform.position - transform.position).normalized;

                playerHealth.TakeDamage(_damage, hitDirection);
            }    
        }
    }
}
