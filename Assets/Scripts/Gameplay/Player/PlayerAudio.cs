using System;
using Zenject;
using UnityEngine;

public sealed class PlayerAudio : IInitializable, IDisposable
{
    private readonly PlayerConfig _playerConfig;
    private readonly PlayerHealth _playerHealth;
    private readonly IAudioService _audioService;
    public PlayerAudio(PlayerConfig playerConfig, PlayerHealth playerHealth, IAudioService audioService)
    {
        _playerConfig = playerConfig;
        _playerHealth = playerHealth;
        _audioService = audioService;
    }
    public void Initialize()
    {
        _playerHealth.OnDamaged += PlayHurt;
        _playerHealth.OnDeath += PlayDeath;
    }
    public void Dispose()
    {
        _playerHealth.OnDamaged -= PlayHurt;
        _playerHealth.OnDeath -= PlayDeath;
    }
    public void PlayWalkStep() { _audioService.PlaySFX(_playerConfig.WalkStep); }
    public void PlayRunStep() { _audioService.PlaySFX(_playerConfig.RunStep); }
    public void  PlayJump() { _audioService.PlaySFX(_playerConfig.JumpClip); }
    private void PlayHurt(Vector2 _) { _audioService.PlaySFX(_playerConfig.HurtClip); }
    private void PlayDeath() { _audioService.PlaySFX(_playerConfig.DeathClip); }
}
