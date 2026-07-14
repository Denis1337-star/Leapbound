using UnityEngine;
using Zenject;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip _walkStep;
    [SerializeField] private AudioClip _runStep;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _hurtClip;
    [SerializeField] private AudioClip _deathClip;

    private IAudioService _audioService;

    [Inject]
    public void Construct(IAudioService audioService)
    {
        _audioService = audioService;
    }

    public void PlayWalkStep()
    {
        _audioService?.PlaySFX(_walkStep);
    }

    public void PlayRunStep()
    {
        _audioService?.PlaySFX(_runStep);
    }

    public void PlayJump()
    {
        _audioService?.PlaySFX(_jumpClip);
    }

    public void PlayHurt()
    {
        _audioService?.PlaySFX(_hurtClip);
    }

    public void PlayDeath()
    {
        _audioService?.PlaySFX(_deathClip);
    }
}
