using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private PlayerMotor _motor;
    private PlayerHealth _health;
    private SpriteRenderer _sprite;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _motor = GetComponent<PlayerMotor>();
        _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<PlayerHealth>();
        _sprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        _animator.SetFloat("Speed", Mathf.Abs(_motor.HorizontalInput));
        _animator.SetBool("IsRun", _motor.IsRunHeld);
        _animator.SetBool("IsCrouch", _motor.IsCrouchingNow);
        _animator.SetBool("IsDead", _health != null && _health.CurrentHP <= 0);
        _animator.SetBool("IsJump", _motor.IsJumpPressed);
    }


    private void LateUpdate()
    {
        if (_rb.linearVelocity.x > 0.1f)
        {
            _sprite.flipX = true;
        }
        else if (_rb.linearVelocity.x < -0.1f)
        {
            _sprite.flipX = false;
        }
    }
}
