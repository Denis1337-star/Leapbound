using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    public AudioSource musicSource; // доступ MusicSource
    public AudioSource sfxSource;   //  SFXSource

    [Header("Music Clips")]
    public AudioClip defaultMusic;  //фон музыки 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;  
            DontDestroyOnLoad(gameObject);  //не уничтожается между сценами
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (musicSource != null && defaultMusic != null)
        {
            PlayMusic(defaultMusic, true);  //проверка и запуск
        }
    }

    // Music
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        //проверка
        if (musicSource == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        //настройка клипа
        musicSource.clip = clip;
        musicSource.loop = loop;
        //вкл аудио
        musicSource.Play();
    }

    public void StopMusic()
    {
        //остановка клипа
        if (musicSource == null) return;
        musicSource.Stop();
    }

    // SFX
    public void PlaySFX(AudioClip clip, float volume = 1f)  //клип + громкость
    {
        if (clip == null || sfxSource == null) return;  //проверка
        sfxSource.PlayOneShot(clip, volume);  //воспроизводит через 
    }
}
