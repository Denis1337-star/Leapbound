using UnityEngine;
using Zenject;

public sealed class PlayerAnimation : ITickable, ILateTickable
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsRunHash = Animator.StringToHash("IsRun");
    private static readonly int IsCrouchHash = Animator.StringToHash("IsCrouch");
    private static readonly int IsDeadHash = Animator.StringToHash("IsDead");
    private static readonly int IsJumpHash = Animator.StringToHash("IsJump");
    private readonly Player _player;
    private readonly PlayerMotor _playerMotor;
    private readonly PlayerHealth _playerHealth;

    public PlayerAnimation(Player player, PlayerMotor playerMotor, PlayerHealth playerHealth)
    {
        _player = player;
        _playerMotor = playerMotor;
        _playerHealth = playerHealth;
    }
    public void Tick()
    {
        Animator animator = _player.Animator;
        animator.SetFloat(SpeedHash, Mathf.Abs(_playerMotor.MoveDirection));
        animator.SetBool(IsRunHash, _playerMotor.IsRunButtonHeld);
        animator.SetBool(IsCrouchHash, _playerMotor.IsCrouching);
        animator.SetBool(IsDeadHash, _playerHealth.CurrentHealth <= 0);
        animator.SetBool(IsJumpHash, _playerMotor.WasJumpPressed);
    }
    public void LateTick()
    {
        float velocityX = _player.Rigidbody.linearVelocity.x;
        if (velocityX > 0.1f)
            _player.SpriteRenderer.flipX = true;
        else if (velocityX < -0.1f)
            _player.SpriteRenderer.flipX = false;
    }
}
