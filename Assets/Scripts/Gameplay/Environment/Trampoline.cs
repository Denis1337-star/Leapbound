using UnityEngine;

public sealed class Trampoline : MonoBehaviour
{
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    [SerializeField] private float _launchForce = 14f;
    [SerializeField] private float _launchDelay = 0.5f;
    [SerializeField] private Animator _animator;
    private float _timer = 0f;
    private bool _isCharging;
    private Player _player;

    private void Update()
    {
        if (!_isCharging)
            return;

        _timer += Time.deltaTime;
        if (_timer < _launchDelay)
            return;

        LaunchPlayer();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isCharging) return;

        if (!collision.collider.TryGetComponent(out Player player))
            return;

        if (player.PlayerHealth.CurrentHealth <= 0)
            return;

        _player = player;
        _isCharging = true;
        _timer = 0f;
        _animator.SetTrigger(JumpHash);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent(out Player player))
            return;

        _player = null;
    }

    private void LaunchPlayer()
    {
        _isCharging = false;
        _timer = 0f;

         if(_player ==null || _player.PlayerHealth.CurrentHealth <=0)
        {
            _player = null;
            return;
        }    

        Vector2 velocity = _player.Rigidbody.linearVelocity;
        velocity.y = _launchForce;
        _player.Rigidbody.linearVelocity = velocity;
        _player = null;
    }
}
