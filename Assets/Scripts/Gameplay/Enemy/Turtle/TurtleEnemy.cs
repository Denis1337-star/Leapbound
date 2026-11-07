using UnityEngine;

public sealed class TurtleEnemy : EnemyBase
{
    private static readonly int IsSpikedHash = Animator.StringToHash("IsSpiked");
    private static readonly int SpikesOutHash = Animator.StringToHash("SpikesOut");
    private static readonly int SpikesInHash = Animator.StringToHash("SpikesIn");
    private enum SpikePhase
    {
        Soft,
        Extending,
        Spiked,
        Retracting
    }
    [SerializeField] private bool _patrolEnable;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _speed;
    [SerializeField] private float _softTime;
    [SerializeField] private float _spikedTime;
    [SerializeField] private float _spikesOutDuration;
    [SerializeField] private float _spikesInDuration;
    private SpikePhase _phase;
    private Vector3 _target;
    private float _timer;
    private bool _spikesActive;
    protected override void Awake()
    {
        base.Awake();
        if (_patrolEnable)
            _target = _pointB.position;

        SetSoft();
    }
    private void FixedUpdate()
    {
        if (IsDead)
            return;

        TickSpikes();

        if (_patrolEnable)
            Patrol();
    }
    private void TickSpikes()
    {
        _timer -= Time.fixedDeltaTime;
        if (_timer > 0f)
            return;

        if (_phase == SpikePhase.Soft)
        {
            _phase = SpikePhase.Extending;
            _spikesActive = true;
            _timer = _spikesOutDuration;
            Animator.SetBool(IsSpikedHash, true);
            Animator.SetTrigger(SpikesOutHash);
            return;
        }

        if (_phase == SpikePhase.Extending)
        {
            _phase = SpikePhase.Spiked;
            _timer = _spikedTime;
            return;
        }

        if (_phase == SpikePhase.Spiked)
        {
            _phase = SpikePhase.Retracting;
            _timer = _spikesInDuration;
            Animator.SetTrigger(SpikesInHash);
            return;
        }

        SetSoft();
    }
    private void SetSoft()
    {
        _phase = SpikePhase.Soft;
        _spikesActive = false;
        _timer = _softTime;
        Animator.SetBool(IsSpikedHash, false);
    }
    private void Patrol()
    {
        Vector2 current = Rigidbody.position;
        Vector2 next = Vector2.MoveTowards(current, _target, _speed * Time.fixedDeltaTime);

        Rigidbody.MovePosition(next);
        FaceMovement(next.x - current.x);

        if (Vector2.Distance(next, _target) > 0.05f)
            return;

        bool atA = Vector2.Distance(_target, _pointA.position) < 0.1f;
        _target = atA ? _pointB.position : _pointA.position;
    }
    private void FaceMovement(float deltaX)
    {
        if (deltaX > 0.01f)
            Sprite.flipX = true;
        else if (deltaX < -0.01f)
            Sprite.flipX = false;
    }
    protected override void OnStomped(Player player)
    {
        if (_spikesActive)
        {
            Vector2 hitDirection = (player.transform.position - transform.position).normalized;
            player.PlayerHealth.TakeDamage(1, hitDirection);
            Vector2 velocity = player.Rigidbody.linearVelocity;
            velocity.y = player.PlayerConfig.StopmBounceForce;
            player.Rigidbody.linearVelocity = velocity;
            return;
        }
        base.OnStomped(player);
    }
}
