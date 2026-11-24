using UnityEngine;

public sealed class SlimeEnemy : EnemyBase
{
    [SerializeField] private bool _patrolEnable;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private SlimeTrail _trailPrefab;
    [SerializeField] private int _trailPoolSize = 16;
    [SerializeField] private float _trailSpacing = 0.45f;
    [SerializeField] private Transform _trailSpawnPoint;

    private Vector3 _target;
    private SlimeTrailPool _trailPool;
    private Vector2 _lastTrailPosition;

    protected override void Awake()
    {
        base.Awake();
        _trailPool = new SlimeTrailPool(_trailPrefab, _trailPoolSize);
        _lastTrailPosition = Rigidbody.position;

        if (_patrolEnable)
            _target = _pointB.position;
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
        Vector2 nextPosition = Vector2.MoveTowards(
            currentPosition, _target, _speed * Time.fixedDeltaTime);

        Rigidbody.MovePosition(nextPosition);
        FaceMovement(nextPosition.x - currentPosition.x);
        TrySpawnTrail(nextPosition);

        if (Vector2.Distance(nextPosition, _target) > 0.05f)
            return;

        bool targetIsPointA = Vector2.Distance(_target, _pointA.position) < 0.1f;
        if (targetIsPointA)
            _target = _pointB.position;
        else
            _target = _pointA.position;
    }
    private void TrySpawnTrail(Vector2 currentPosition)
    {
        if (Vector2.Distance(currentPosition, _lastTrailPosition) < _trailSpacing)
            return;

        _lastTrailPosition = currentPosition;
        Vector3 spawnPosition;
        if (_trailSpawnPoint != null)
            spawnPosition = _trailSpawnPoint.position;
        else
            spawnPosition = new Vector3(currentPosition.x, currentPosition.y, 0f);

        _trailPool.Play(spawnPosition);
    }
}