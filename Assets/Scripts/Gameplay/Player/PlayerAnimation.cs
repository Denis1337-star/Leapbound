using UnityEngine;
using Zenject;

public sealed class PlayerAnimation : ITickable, ILateTickable
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsRunHash = Animator.StringToHash("IsRun");
    private static readonly int IsDeadHash = Animator.StringToHash("IsDead");
    private static readonly int IsJumpHash = Animator.StringToHash("IsJump");
    private static readonly int IsDoubleJumpHash = Animator.StringToHash("IsDoubleJump");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int AttackKindHash = Animator.StringToHash("AttackKind");
    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");
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
        animator.SetBool(IsDeadHash, _playerHealth.CurrentHealth <= 0);
        animator.SetBool(IsJumpHash, !_playerMotor.IsOnGround && !_playerMotor.IsDoubleJumping);
        animator.SetBool(IsDoubleJumpHash, _playerMotor.IsDoubleJumping);
    }
    public void LateTick()
    {
        float velocityX = _player.Rigidbody.linearVelocity.x;
        if (velocityX > 0.1f)
            _player.SpriteRenderer.flipX = false;
        else if (velocityX < -0.1f)
            _player.SpriteRenderer.flipX = true;
    }
    public void PlayAttack(int kind)
    {
        Animator animator = _player.Animator;
        animator.SetInteger(AttackKindHash, kind);
        animator.SetBool(IsAttackingHash, true);
        animator.SetTrigger(AttackHash);
    }
    public void StopAttack()
    {
        _player.Animator.SetBool(IsAttackingHash, false);
        _player.Animator.ResetTrigger(AttackHash);
    }
    public void SetUnscaled(bool unscaled)
    {
        if (unscaled)
            _player.Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        else
            _player.Animator.updateMode = AnimatorUpdateMode.Normal;
    }
}

