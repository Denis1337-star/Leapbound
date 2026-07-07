using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private PlayerInput _input;
    private PlayerCrouch _crouch;
    private PlayerHealth _health;
    private SpriteRenderer _sprite;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<PlayerInput>();
       _rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<PlayerHealth>();
        _crouch = GetComponent<PlayerCrouch>();
        _sprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        //Устанавливает парраметр speed в Animator модулем 
        _animator.SetFloat("Speed", Mathf.Abs(_input.Move));

        //Устанавливает булевые парраметры 
        _animator.SetBool("IsRun",_input.RunHeld);
       _animator.SetBool("IsCrouch", _crouch != null && _crouch.IsCrouching);
        _animator.SetBool("IsDead",_health!= null && _health.CurrentHP <= 0);
        _animator.SetBool("IsJump", _input.JumpPressed);
    }


    private void LateUpdate()
    {
        //Флип спрайт
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
