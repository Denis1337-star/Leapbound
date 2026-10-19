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
    private readonly PlayerDust _playerDust;
    public float MoveDirection { get; private set; }
    public bool IsRunButtonHeld { get; private set; }
    public bool WasJumpPressed { get; private set; }
    public bool IsOnGround { get; private set; }
    public bool IsDoubleJumping { get; private set; }
    private bool _wasOnGround;
    private int _jumpsLeft;
    
    public PlayerMotor(Player player, PlayerConfig playerConfig, IInputService inputService, PlayerAudio playerAudio,
        IGameStateService gameStateService, PlayerDamageFeedback playerDamageFeedback, PlayerDust playerDust)
    {
        _player = player;
        _playerConfig = playerConfig;
        _inputService = inputService;
        _playerAudio = playerAudio;
        _gameStateService = gameStateService;
        _playerDamageFeedback = playerDamageFeedback;
        _jumpsLeft = _playerConfig.MaxJumpCount;
        _playerDust = playerDust;
    }
    public void Tick()
    {
        if (_player.PlayerHealth.CurrentHealth <= 0)
            return;
        if (_gameStateService.CurrentState != GameState.Playing)
            return;

        ReadInput();
        ProcessJump();
    }
    public void FixedTick()
    {
        if (_player.PlayerHealth.CurrentHealth <= 0)
            return;
        if (_gameStateService.CurrentState != GameState.Playing)
            return;

        ApplyMovement();
        bool runDust = IsOnGround && IsRunButtonHeld && Mathf.Abs(MoveDirection) > 0.01f;
        bool flipX = MoveDirection < 0f;
        _playerDust.SetRunning(runDust, flipX);
        
    }
    private void ReadInput()
    {
        MoveDirection = _inputService.Move;
        IsRunButtonHeld = _inputService.RunHeld;
        WasJumpPressed = _inputService.JumpPressed;
    }

    private void ProcessJump()
    {
        IsOnGround = Physics2D.OverlapCircle(_player.GroundCheckAnchor.position,
_playerConfig.GroundCheckRadius, _playerConfig.GroundLayer);

        if (IsOnGround && !_wasOnGround)
        {
            _jumpsLeft = _playerConfig.MaxJumpCount;
            _playerDust.PlayLand();
        }

        _wasOnGround = IsOnGround;
        if (IsOnGround)
            IsDoubleJumping = false;

        if (!WasJumpPressed || _jumpsLeft <= 0)
            return;

        bool isAirJump = !IsOnGround;

        Vector2 velocity = _player.Rigidbody.linearVelocity;
        velocity.y = 0f;
        _player.Rigidbody.linearVelocity = velocity;

        _player.Rigidbody.AddForce(Vector2.up * _playerConfig.JumpForce, ForceMode2D.Impulse);
        _jumpsLeft--;
        IsDoubleJumping = isAirJump;
        _playerAudio.PlayJump();
        _playerDust.PlayJump();
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

        Vector2 velocity = _player.Rigidbody.linearVelocity;
        velocity.x = MoveDirection * moveSpeed + _player.PlatformVelocity.x;
        velocity.y += _player.PlatformVelocity.y;
        _player.Rigidbody.linearVelocity = velocity;
    }
}
