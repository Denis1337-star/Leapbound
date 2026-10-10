using UnityEngine;
public sealed class EnemyAudio 
{
    private readonly IAudioService _audioService;
    private readonly AudioClip _hurtClip;
    private readonly AudioClip _deathClip;

    public EnemyAudio(IAudioService audioService, AudioClip hurtClip, AudioClip deathClip)
    {
        _audioService = audioService;
        _hurtClip = hurtClip;
        _deathClip = deathClip;
    }

    public void PlayHurt(Vector2 _) { Play(_hurtClip); }

    public void PlayDeath() { Play(_deathClip); }

    private void Play(AudioClip clip) { _audioService.PlaySFX(clip); }
}
