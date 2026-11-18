using UnityEngine;

public sealed class FireTrap : MonoBehaviour
{
    private enum FireTrapState { Idle, Warn, Active }
    private static readonly int IsWarningHash = Animator.StringToHash("IsWarning");
    private static readonly int IsFiringHash = Animator.StringToHash("IsFiring");

    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _fireHitbox;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _idleTime = 2f;
    [SerializeField] private float _warnTime = 0.6f;
    [SerializeField] private float _activeTime = 1.2f;
    private int _damage = 3;
    private FireTrapState _fireTrapState;
    private float _timer;
    private bool _hitThisCycle;

    private void Awake()
    {
        _fireHitbox.enabled = false;
        EnterIdle();
        _timer += _startDelay;
    }
    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer > 0f)
            return;

        if (_fireTrapState == FireTrapState.Idle)
            EnterWarn();
        else if (_fireTrapState == FireTrapState.Warn)
            EnterActive();
        else
            EnterIdle();
    }
    public void TryHit(Collider2D other)
    {
        if (_fireTrapState != FireTrapState.Active || _hitThisCycle)
            return;

        if (!other.TryGetComponent(out Player player))
            return;

        if (player.PlayerHealth.CurrentHealth <= 0)
            return;

        Vector2 dir = (player.transform.position - transform.position).normalized;
        player.PlayerHealth.TakeDamage(_damage, dir);
        _hitThisCycle = true;
    }
    private void EnterIdle()
    {
        _fireTrapState = FireTrapState.Idle;
        _timer = _idleTime;
        _hitThisCycle = false;
        _fireHitbox.enabled = false;

        _animator.SetBool(IsWarningHash, false);
        _animator.SetBool(IsFiringHash, false);
    }
    private void EnterWarn()
    {
        _fireTrapState = FireTrapState.Warn;
        _timer = _warnTime;
        _fireHitbox.enabled = false;

        _animator.SetBool(IsWarningHash, true);
        _animator.SetBool(IsFiringHash, false);
    }
    private void EnterActive()
    {
        _fireTrapState = FireTrapState.Active;
        _timer = _activeTime;
        _hitThisCycle = false;
        _fireHitbox.enabled = true;

        _animator.SetBool(IsWarningHash, false);
        _animator.SetBool(IsFiringHash, true);
    }
}
