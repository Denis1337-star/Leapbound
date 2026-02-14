using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance {get; private set;}

    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    public float GetMusicValue() => musicVolume;
    public float GetSFXValue() => sfxVolume;

    public void SetMusicValue(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);

        if (AudioManager.Instance)
            AudioManager.Instance.SetMusicVolume(value);
    }

    public void SetSFXValue(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);

        if (AudioManager.Instance)
            AudioManager.Instance.SetSFXVolume(value);
    }
}

