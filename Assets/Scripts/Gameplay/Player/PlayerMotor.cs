using UnityEngine;
using Zenject;

public sealed class PlayerMotor : ITickable, IFixedTickable
{
    private readonly Player _player;
    private readonly PlayerConfig _playerConfig;
    private readonly IInputService _inputService;
    private readonly PlayerAudio _playerAudio;
    private readonly IGameStateService _gameStateService;
    private readonly PlayerDamageFeedback _playerDamageFeedback;
    public float MoveDirection { get; private set; }
    public bool IsRunButtonHeld { get; private set; }
    public bool IsCrouchButtonHeld { get; private set; }
    public bool WasJumpPressed { get; private set; }
    public bool IsOnGround { get; private set; }
    public bool IsCrouching { get; private set; }
    public bool IsBlockedByCeiling { get; private set; }
    public PlayerMotor(Player player, PlayerConfig playerConfig, IInputService inputService, PlayerAudio playerAudio,
        IGameStateService gameStateService, PlayerDamageFeedback playerDamageFeedback)
    {
        _player = player;
        _playerConfig = playerConfig;
        _inputService = inputService;
        _playerAudio = playerAudio;
        _gameStateService = gameStateService;
        _playerDamageFeedback = playerDamageFeedback;
    }
    public void Tick()
    {
        if (_player.PlayerHealth.CurrentHealth <= 0)
            return;
        if (_gameStateService.CurrentState != GameState.Playing)
            return;

        ReadInput();
        UpdateCrouchState();
        ProcessJump();
    }
    public void FixedTick()
    {
        if (_player.PlayerHealth.CurrentHealth <= 0)
            return;
        if (_gameStateService.CurrentState != GameState.Playing)
            return;

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
        IsOnGround = Physics2D.OverlapCircle(_player.GroundCheckAnchor.position,
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
        if (!WasJumpPressed || !IsOnGround || IsCrouching)
            return;

        _player.Rigidbody.AddForce(Vector2.up * _playerConfig.JumpForce, ForceMode2D.Impulse);
        _playerAudio.PlayJump();
        _inputService.ConsumeJump();
    }
    private void ApplyMovement()
    {
        if (_playerDamageFeedback.IsInKnockback)
            return;

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
