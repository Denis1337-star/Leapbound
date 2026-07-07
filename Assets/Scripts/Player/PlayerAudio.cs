using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip _walkStep;
    [SerializeField] private AudioClip _runStep;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _hurtClip;
    [SerializeField] private AudioClip _deathClip;

    public void PlayWalkStep()
    {
        AudioManager.Instance?.PlaySFX(_walkStep);
    }

    public void PlayRunStep()
    {
        AudioManager.Instance?.PlaySFX(_runStep);
    }

    public void PlayJump()
    {
        AudioManager.Instance?.PlaySFX(_jumpClip);
    }

    public void PlayHurt()
    {
        AudioManager.Instance?.PlaySFX(_hurtClip);
    }

    public void PlayDeath()
    {
        AudioManager.Instance?.PlaySFX(_deathClip);
    }
}
