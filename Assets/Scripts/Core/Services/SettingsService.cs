using UnityEngine;
using Zenject;

public  sealed class SettingsService : ISettingsService, IInitializable
{
    private readonly IAudioService _audioService;

    private float _musicVolume = 1f;
    private float _sfxVolume = 1f;

    public SettingsService (IAudioService audioService)
    {
        _audioService = audioService;
    }
    public void Initialize()
    {
        _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        _audioService.SetMusicVolume(_musicVolume);
        _audioService.SetSFXVolume(_sfxVolume);
    }
    public float GetMusicValue()
    { 
        return _musicVolume;
    }
    public float GetSFXValue()
    {
        return _sfxVolume;
    }
    public void SetMusicValue(float value)
    {
        _musicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume",value);
        _audioService.SetMusicVolume(value);
    }
    public void SetSFXValue(float value)
    {
        _sfxVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        _audioService.SetSFXVolume(value);
    }
}
