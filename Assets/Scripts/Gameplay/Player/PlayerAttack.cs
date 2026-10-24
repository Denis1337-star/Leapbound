using UnityEngine;
using Zenject;

public sealed class PlayerAttack : ITickable
{
    private readonly Player _player;
    private readonly PlayerMotor _playerMotor;
    private readonly PlayerConfig _playerConfig;
    private readonly IInputService _inputService;
    private readonly IGameStateService _gameStateService;
    private readonly PlayerDamageFeedback _playerDamageFeedback;
    private readonly PlayerAnimation _playerAnimation;
    private bool _isAttacking;
    private bool _hitActive;
    private float _elapsed;
    private float _duration;
    public PlayerAttack(Player player, PlayerMotor playerMotor, PlayerConfig playerConfig,
        IInputService inputService, IGameStateService gameStateService,
        PlayerDamageFeedback playerDamageFeedback, PlayerAnimation playerAnimation)
    {
        _player = player;
        _playerMotor = playerMotor;
        _playerConfig = playerConfig;
        _inputService = inputService;
        _gameStateService = gameStateService;
        _playerDamageFeedback = playerDamageFeedback;
        _playerAnimation = playerAnimation;
    }
    public void Tick()
    {
        if (_player.PlayerHealth.CurrentHealth <= 0 || _gameStateService.CurrentState != GameState.Playing)
        {
            Cancel();
            _inputService.ConsumeAttack();
            return;
        }
        if (_playerDamageFeedback.IsInKnockback || !_playerMotor.IsOnGround)
        {
            Cancel();
            _inputService.ConsumeAttack();
            return;
        }
        if (_isAttacking)
        {
            UpdateAttack();
            _inputService.ConsumeAttack();
            return;
        }
        if (!_inputService.AttackPressed)
            return;
        _inputService.ConsumeAttack();
        Begin();
    }
    private void Begin()
    {
        int kind = 0;
        if (Mathf.Abs(_playerMotor.MoveDirection) > 0.1f)
        {
            if (_playerMotor.IsRunButtonHeld)
                kind = 2;
            else
                kind = 1;
        }

        if (kind == 1)
            _duration = _playerConfig.AttackWalkDuration;
        else if (kind == 2)
            _duration = _playerConfig.AttackRunDuration;
        else
            _duration = _playerConfig.AttackIdleDuration;

        _elapsed = 0f;
        _hitActive = false;
        _isAttacking = true;
        _player.SetAttackMoveLock(kind == 0);

        Animator animator = _player.Animator;
        _playerAnimation.PlayAttack(kind);
            
    }
    private void UpdateAttack()
    {
        _elapsed += Time.deltaTime;
        if (!_hitActive && _elapsed >= _playerConfig.AttackHitStart)
        {
            _hitActive = true;
            _player.SwordHit.Begin();
        }
        if (_hitActive && _elapsed >= _playerConfig.AttackHitEnd)
        {
            _hitActive = false;
            _player.SwordHit.End();
        }
        if (_elapsed < _duration)
            return;
        Cancel();
    }
    private void Cancel()
    {
        if (!_isAttacking && !_hitActive)
            return;
        _isAttacking = false;
        _hitActive = false;
        _player.SetAttackMoveLock(false);
        _player.SwordHit.End();
        _playerAnimation.StopAttack();
    }
}
