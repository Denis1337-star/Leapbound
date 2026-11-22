using UnityEngine;

public sealed class ChickenEnemy : EnemyBase
{
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    [SerializeField] private BoxCollider2D _zone;
    [SerializeField] private float _chaseSpeed = 3.5f;
    [SerializeField] private float _sightHeight = 0.75f;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _attackRecoverTime = 2f;
    private float _recoverTimer;
    private Player _playerInSight;
    private readonly Collider2D[] _overlap = new Collider2D[4];
    private ContactFilter2D _playerFilter;

    protected override void Awake()
    {
        base.Awake();
        _playerFilter = new ContactFilter2D();
        _playerFilter.SetLayerMask(_playerLayer);
        _playerFilter.useTriggers = true;
    }

    private void FixedUpdate()
    {
        if (IsDead)
            return;

        if (_recoverTimer > 0f)
        {
            _recoverTimer -= Time.fixedDeltaTime;
            Idle();
            return;
        }

        UpdateSight();
        if (_playerInSight != null)
            Chase();
        else
            Idle();
    }
    private void UpdateSight()
    {
        _playerInSight = null;
        Bounds zoneBounds = _zone.bounds;
        int count = Physics2D.OverlapBox(
            zoneBounds.center, zoneBounds.size, 0f, _playerFilter, _overlap);
        for (int i = 0; i < count; i++)
        {
            if (!_overlap[i].TryGetComponent(out Player player))
                continue;

            if (player.PlayerHealth.CurrentHealth <= 0)
                continue;

            float verticalOffset = Mathf.Abs(
                player.transform.position.y - transform.position.y);

            if (verticalOffset > _sightHeight)
                continue;

            _playerInSight = player;
            break;
        }
    }
    private void Chase()
    {
        Animator.SetBool(IsRunningHash, true);

        float halfWidth = BodyCollider.bounds.extents.x;
        float minX = _zone.bounds.min.x + halfWidth;
        float maxX = _zone.bounds.max.x - halfWidth;
        float targetX = Mathf.Clamp(_playerInSight.transform.position.x, minX, maxX);

        Vector2 current = Rigidbody.position;
        Vector2 next = Vector2.MoveTowards(
            current, new Vector2(targetX, current.y), _chaseSpeed * Time.fixedDeltaTime);

        Rigidbody.MovePosition(next);
        FaceMovement(next.x - current.x);
    }
    protected override void OnDamagedPlayer(Player player)
    {
        _recoverTimer = _attackRecoverTime;
        Idle();
    }
    private void Idle()
    {
        Animator.SetBool(IsRunningHash, false);
    }
}