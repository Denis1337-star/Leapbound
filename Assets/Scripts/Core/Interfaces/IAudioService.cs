using UnityEngine;

public interface IAudioService 
{
    void PlayMusic(AudioClip clip, bool loop = true);
    void PlaySFX(AudioClip clip);
    void SetMusicVolume(float value);
    void SetSFXVolume(float value);
}
