using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip walkStep;
    public AudioClip runStep;
    public AudioClip jumpClip;
    public AudioClip hurtClip;
    public AudioClip deathClip;

    public void PlayWalkStep()
    {
        AudioManager.Instance?.PlaySFX(walkStep);
    }

    public void PlayRunStep()
    {
        AudioManager.Instance?.PlaySFX(runStep);
    }

    public void PlayJump()
    {
        AudioManager.Instance?.PlaySFX(jumpClip);
    }

    public void PlayHurt()
    {
        AudioManager.Instance?.PlaySFX(hurtClip);
    }

    public void PlayDeath()
    {
        AudioManager.Instance?.PlaySFX(deathClip);
    }
}
