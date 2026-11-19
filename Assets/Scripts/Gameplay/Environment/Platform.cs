using UnityEngine;

public sealed class Platform : MonoBehaviour
{
    private enum PlatformMode { Patrol, OnDemand }
    private enum OnDemandState { Waiting, Moving }
    private enum MoveReason { None, Call, Patrol }

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private PlatformMode _platformMode = PlatformMode.Patrol;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _emptySpeed = 6f;
    [SerializeField] private float _boardedSpeed = 2f;
    [SerializeField] private float _passengerExitGrace = 0.35f;
    public Vector2 Velocity { get; private set; }
    private Vector2 _target;
    private OnDemandState _onDemandState;
    private MoveReason _moveReason;
    private bool _hasPassenger;
    private float _passengerLostTimer = -1f;
    private readonly ContactPoint2D[] _contacts = new ContactPoint2D[8];

    private void Awake()
    {
        if (_platformMode == PlatformMode.Patrol)
        {
            _target = _pointB.position;
            return;
        }
        _rigidbody.position = _pointA.position;
        _target = _pointA.position;
        EnterWaiting();
    }
    private void FixedUpdate()
    {
        if (_platformMode == PlatformMode.Patrol)
        {
            if (MoveTowardsTarget(_speed))
                SetOppositeTargetFrom(_target);
            return;
        }
        TickPassengerGrace();
        TickOnDemand();
    }
    private void TickPassengerGrace()
    {
        if (_passengerLostTimer < 0f)
            return;

        _passengerLostTimer -= Time.fixedDeltaTime;
        if (_passengerLostTimer > 0f)
            return;

        _passengerLostTimer = -1f;
        _hasPassenger = false;
    }
    private void TickOnDemand()
    {
        if (_onDemandState == OnDemandState.Waiting)
        {
            Velocity = Vector2.zero;
            if (!_hasPassenger)
                return;

            SetOppositeTargetFrom(_rigidbody.position);
            _moveReason = MoveReason.Patrol;
            _onDemandState = OnDemandState.Moving;
        }

        float moveSpeed;
        if (_moveReason == MoveReason.Call)
            moveSpeed = _emptySpeed;
        else
            moveSpeed = _boardedSpeed;

        if (!MoveTowardsTarget(moveSpeed))
            return;

        if (_moveReason == MoveReason.Call)
        {
            EnterWaiting();
            return;
        }
        if (_hasPassenger)
        {
            SetOppositeTargetFrom(_target);
            return;
        }
        EnterWaiting();
    }
    private bool MoveTowardsTarget(float moveSpeed)
    {
        Vector2 current = _rigidbody.position;
        Vector2 next = Vector2.MoveTowards(current, _target, moveSpeed * Time.fixedDeltaTime);
        Velocity = (next - current) / Time.fixedDeltaTime;
        _rigidbody.MovePosition(next);
        return Vector2.Distance(next, _target) <= 0.05f;
    }
    private void SetOppositeTargetFrom(Vector2 fromPoint)
    {
        bool nearPointA = Vector2.Distance(fromPoint, _pointA.position) < 0.15f;
        if (nearPointA)
            _target = _pointB.position;
        else
            _target = _pointA.position;
    }
    private void EnterWaiting()
    {
        _moveReason = MoveReason.None;
        _onDemandState = OnDemandState.Waiting;
        Velocity = Vector2.zero;
    }
    public void CallToPointA(){TryCall(_pointA.position); }
    public void CallToPointB(){TryCall(_pointB.position);}
    private void TryCall(Vector2 point)
    {
        if (_platformMode != PlatformMode.OnDemand)
            return;

        if (_onDemandState != OnDemandState.Waiting)
            return;

        if (Vector2.Distance(_rigidbody.position, point) < 0.1f)
            return;

        _target = point;
        _moveReason = MoveReason.Call;
        _onDemandState = OnDemandState.Moving;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_platformMode != PlatformMode.OnDemand)
            return;

        if (!collision.collider.TryGetComponent(out Player player))
            return;

        int count = collision.GetContacts(_contacts);
        for (int i = 0; i < count; i++)
        {
            if (_contacts[i].normal.y >= -0.5f)
                continue;

            _hasPassenger = true;
            _passengerLostTimer = -1f;
            return;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_platformMode != PlatformMode.OnDemand)
            return;

        if (!collision.collider.TryGetComponent(out Player player))
            return;

        _passengerLostTimer = _passengerExitGrace;
    }
}

