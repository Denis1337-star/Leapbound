using UnityEngine;

public sealed class FatBirdEnemy : EnemyBase
{
    private static readonly int IsFallingHash = Animator.StringToHash("IsFalling");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private enum BirdState
    {
        Hover,
        Fall,
        Grounded,
        Rise
    }
    [SerializeField] private bool _diveEnable;
    [SerializeField] private float _hoverTime;
    [SerializeField] private float _fallSpeed;
    [SerializeField] private float _riseSpeed;
    [SerializeField] private float _groundWait;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius;

    private BirdState _birdState;
    private Vector2 _homePosition;
    private float _timer;

    protected override void Awake()
    {
        base.Awake();
        _homePosition = Rigidbody.position;
        _birdState = BirdState.Hover;
        _timer = _hoverTime;
        SetAnimation(false, false);
    }
    private void FixedUpdate()
    {
        if (IsDead)
            return;

        if (!_diveEnable)
            return;

        if (_birdState == BirdState.Hover)
            TickHover();
        else if (_birdState == BirdState.Fall)
            TickFall();
        else if (_birdState == BirdState.Grounded)
            TickGrounded();
        else
            TickRise();
            
    }
    private void TickHover()
    {
        _timer -= Time.fixedDeltaTime;
        if (_timer > 0f)
            return;

        _birdState = BirdState.Fall;
        SetAnimation(true, false);
    }
    private void TickFall()
    {
        Vector2 next = Rigidbody.position + Vector2.down * _fallSpeed * Time.fixedDeltaTime;
        Rigidbody.MovePosition(next);
        if (!IsOnGround())
            return;
        _birdState = BirdState.Grounded;
        _timer = _groundWait;
        SetAnimation(false, true);
    }
    private void TickGrounded()
    {
        _timer -= Time.fixedDeltaTime;
        if (_timer > 0f)
            return;

        _birdState = BirdState.Rise;
        SetAnimation(false, false);
    }
    private void TickRise()
    {
        Vector2 next = Vector2.MoveTowards(Rigidbody.position,
                  _homePosition, _riseSpeed * Time.fixedDeltaTime);
        Rigidbody.MovePosition(next);

        if (Vector2.Distance(next, _homePosition) > 0.05f)
            return;

        Rigidbody.position = _homePosition;
        _birdState = BirdState.Hover;
        _timer = _hoverTime;
        SetAnimation(false, false);
    }
    private bool IsOnGround()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
    }
    private void SetAnimation(bool falling, bool grounded)
    {
        Animator.SetBool(IsFallingHash, falling);
        Animator.SetBool(IsGroundedHash, grounded);
    }
    private void OnDrawGizmosSelected()
    {
        if (_groundCheck == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
}
