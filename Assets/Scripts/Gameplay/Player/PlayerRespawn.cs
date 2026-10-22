using UnityEngine;

public sealed class PlayerRespawn 
{
    private readonly Player _player;
    private readonly PlayerHealth _playerHealth;
    private readonly PlayerMotor _playerMotor;
    private readonly PlayerDamageFeedback _playerDamageFeedback;
    private readonly CheckpointService _checkpointService;

    public PlayerRespawn(Player player, PlayerHealth playerHealth, PlayerMotor playerMotor, PlayerDamageFeedback playerDamageFeedback,
        CheckpointService checkpointService)
    {
        _player = player;
        _playerHealth = playerHealth;
        _playerMotor = playerMotor;
        _playerDamageFeedback = playerDamageFeedback;
        _checkpointService = checkpointService;
    }
    public void Respawn()
    {
        _playerHealth.Revive();
        _playerMotor.ResetMotion();
        _playerDamageFeedback.OnDeath();
        _player.RestoreBody(_checkpointService.SpawnPosition);
        _player.SpriteRenderer.enabled = true;
        _player.Animator.updateMode = AnimatorUpdateMode.Normal;
    }
}
