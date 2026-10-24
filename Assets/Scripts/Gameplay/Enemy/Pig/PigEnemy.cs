using UnityEngine;

public sealed class PigEnemy : EnemyBase
{
    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
    [SerializeField] private bool _patrolEnable;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _speed = 2f;
    private Vector3 _target;

    protected override void Awake()
    {
        base.Awake();
        if (_patrolEnable)
            _target = _pointB.position;

        Animator.SetBool(IsWalkingHash, _patrolEnable);
    }

    private void FixedUpdate()
    {
        if (IsDead)
            return;

        if (!_patrolEnable)
            return;

        Patrol();
    }

    private void Patrol()
    {
        Vector2 currentPosition = Rigidbody.position;
        Vector2 nextPosition = Vector2.MoveTowards(currentPosition, _target,
            _speed * Time.fixedDeltaTime);

        Rigidbody.MovePosition(nextPosition);
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
}
