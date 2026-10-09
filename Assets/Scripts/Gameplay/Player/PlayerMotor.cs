using UnityEngine;
using Zenject;

public sealed class PlayerMotor : ITickable, IFixedTickable
{
    private readonly Player _player;
    private readonly PlayerConfig _playerConfig;
    private readonly IInputService _inputService;
    private readonly PlayerAudio _playerAudio;
    public float MoveDirection { get; private set; }
    public bool IsRunButtonHeld { get; private set; }
    public bool IsCrouchButtonHeld { get; private set; }
    public bool WasJumpPressed { get; private set; }
    public bool IsOnGrounded { get; private set; }
    public bool IsCrouching { get; private set; }
    public bool IsBlockedByCeiling { get; private set; }
    public PlayerMotor(Player player, PlayerConfig playerConfig, IInputService inputService, PlayerAudio playerAudio)
    {
        _player = player;
        _playerConfig = playerConfig;
        _inputService = inputService;
        _playerAudio = playerAudio;
    }
    public void Tick()
    {
        if (_player.PlayerHealth.CurrentHealth <= 0)
            return;

        ReadInput();
        UpdateCrouchState();
        ProcessJump();
    }
    public void FixedTick()
    {
        ApplyMovement();
    }
    private void ReadInput()
    {
        MoveDirection = _inputService.Move;
        IsRunButtonHeld = _inputService.RunHeld;
        IsCrouchButtonHeld = _inputService.CrouchHeld;
        WasJumpPressed = _inputService.JumpPressed;
    }
    private void UpdateCrouchState()
    {
        IsOnGrounded = Physics2D.OverlapCircle(_player.GroundCheckAnchor.position,
    _playerConfig.GroundCheckRadius, _playerConfig.GroundLayer);

        IsBlockedByCeiling = Physics2D.OverlapCircle(_player.CeilingCheckAnchor.position,
            _playerConfig.CeilingCheckRadius, _playerConfig.GroundLayer);

        bool shouldCrouch = IsCrouchButtonHeld || IsBlockedByCeiling;
        if (shouldCrouch != IsCrouching)
        {
            IsCrouching = shouldCrouch;
            _player.SetCrouchCollider(IsCrouching);
        }
    }
    private void ProcessJump()
    {
        if (!WasJumpPressed || !IsOnGrounded || IsCrouching)
            return;

        _player.Rigidbody.AddForce(Vector2.up * _playerConfig.JumpForce, ForceMode2D.Impulse);
        _playerAudio.PlayJump();
        _inputService.ConsumeJump();
    }
    private void ApplyMovement()
    {
        float moveSpeed;
        if (IsRunButtonHeld)
            moveSpeed = _playerConfig.RunSpeed;
        else
            moveSpeed = _playerConfig.WalkSpeed;

        if (IsCrouching)
            moveSpeed *= _playerConfig.CrawlSpeedMultiplier;

        Vector2 velocity = _player.Rigidbody.linearVelocity;
        velocity.x = MoveDirection * moveSpeed + _player.PlatformVelocity.x;
        velocity.y += _player.PlatformVelocity.y;
        _player.Rigidbody.linearVelocity = velocity;
    }
}
