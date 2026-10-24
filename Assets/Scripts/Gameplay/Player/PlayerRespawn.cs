using UnityEngine;

public sealed class PlayerRespawn 
{
    private readonly Player _player;
    private readonly PlayerHealth _playerHealth;
    private readonly PlayerMotor _playerMotor;
    private readonly PlayerDamageFeedback _playerDamageFeedback;
    private readonly CheckpointService _checkpointService;
    private readonly PlayerAnimation _playerAnimation;

    public PlayerRespawn(Player player, PlayerHealth playerHealth, PlayerMotor playerMotor, PlayerDamageFeedback playerDamageFeedback,
        CheckpointService checkpointService, PlayerAnimation playerAnimation)
    {
        _player = player;
        _playerHealth = playerHealth;
        _playerMotor = playerMotor;
        _playerDamageFeedback = playerDamageFeedback;
        _checkpointService = checkpointService;
        _playerAnimation = playerAnimation;
    }
    public void Respawn()
    {
        _playerHealth.Revive();
        _playerMotor.ResetMotion();
        _playerDamageFeedback.OnDeath();
        _player.RestoreBody(_checkpointService.SpawnPosition);
        _player.SpriteRenderer.enabled = true;
        _playerAnimation.SetUnscaled(false);
    }
}
