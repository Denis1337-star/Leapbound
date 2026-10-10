using UnityEngine;

public sealed class PigEnemy : EnemyBase
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private int _damage = 50;
    private Vector3 _target;

    protected override void Awake()
    {
        base.Awake();
        _target = _pointB.position;
    }

    private void FixedUpdate()
    {
        if (IsDead)
            return;

        Patrol();
    }

    private void Patrol()
    {
        Vector2 currentPosition = _rigidbody.position;
        Vector2 nextPosition = Vector2.MoveTowards(currentPosition, _target,
            _speed * Time.fixedDeltaTime);

        _rigidbody.MovePosition(nextPosition);
        FaceMovement(nextPosition.x - currentPosition.x);

        if (Vector2.Distance(nextPosition, _target) > 0.05f)
            return;

        bool targetIsPointA = Vector2.Distance(_target, _pointA.position) < 0.1f;
        if (targetIsPointA)
            _target = _pointB.position;
        else
            _target = _pointA.position;
       
    }
    private void FaceMovement(float deltaX)
    {
        if (deltaX > 0.01f)
            Sprite.flipX = true;
        else if (deltaX < -0.01f)
            Sprite.flipX = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent(out Player player))
            return;

        float playerBottom = player.BodyCollider.bounds.min.y;
        float enemyTop = BodyCollider.bounds.max.y;


        if (playerBottom > enemyTop - 0.05f)
        {
            TakeDamage(MaxHealth, Vector2.up);
            return;
        }

        Vector2 hitDirection = (player.transform.position - transform.position).normalized;
        player.PlayerHealth.TakeDamage(_damage, hitDirection);
    }
}
