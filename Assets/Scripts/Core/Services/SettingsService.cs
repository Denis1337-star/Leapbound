using UnityEngine;
using Zenject;

public  sealed class SettingsService : ISettingsService, IInitializable
{
    private readonly IAudioService _audioService;

    private float _musicVolume = 1f;
    private float _sfxVolume = 1f;
    public float MusicVolume => _musicVolume;
    public float SfxVolume => _sfxVolume;

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
    public void SetMusicVolume(float value)
    {
        _musicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume",value);
        _audioService.SetMusicVolume(value);
    }
    public void SetSfxVolume(float value)
    {
        _sfxVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        _audioService.SetSFXVolume(value);
    }
}
