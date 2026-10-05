using UnityEngine;

public class AudioManager : MonoBehaviour, IAudioService
{
    [Header("Sources")]
    [SerializeField] private AudioSource _musicSource; 
    [SerializeField] private AudioSource _sfxSource;

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (!clip) return;  

        _musicSource.clip = clip;  
       _musicSource.loop = loop;  
        _musicSource.Play();      
    }

    public void PlaySFX(AudioClip clip) 
    {
        if (!clip) return;  

        _sfxSource.PlayOneShot(clip);  
    }

    public void SetMusicVolume(float value)
    {
        _musicSource.volume = value; 
    }

    public void SetSFXVolume(float value)
    {
        _sfxSource.volume = value;   
    }
}
