using UnityEngine;
public sealed class EnemyAudio 
{
    private readonly IAudioService _audioService;
    private readonly AudioClip _deathClip;

    public EnemyAudio(IAudioService audioService, AudioClip deathClip)
    {
        _audioService = audioService;
        _deathClip = deathClip;
    }

    public void PlayDeath() { _audioService.PlaySFX(_deathClip); }
}
