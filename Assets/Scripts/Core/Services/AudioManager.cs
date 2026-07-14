using UnityEngine;

public class AudioManager : ValidatedMonoBehaviour, IAudioService
{
    [Header("Sources")]
    [SerializeField] private AudioSource _musicSource; 
    [SerializeField] private AudioSource _sfxSource;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override bool ValidateInternal()
    {
        bool valid = true;
        valid &= ValidationUtility.IsAssigned(this, _musicSource, nameof(_musicSource));
        valid &= ValidationUtility.IsAssigned(this, _sfxSource, nameof(_sfxSource));
        return valid;
    }

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
