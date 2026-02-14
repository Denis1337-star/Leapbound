using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {  get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource; // доступ MusicSource  Фоновая
    [SerializeField] private AudioSource sfxSource;   //  SFXSource   Звуковые эффекты

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;  //Назначает текущий обьект как единственный 

        DontDestroyOnLoad(gameObject); //Сохраняет обьект между сценами
    }

    // Music  фоновой
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (!clip) return;  //если нету клипа = выход

        musicSource.clip = clip;  //Назначаем аудио источнику
        musicSource.loop = loop;  //зацикливаем
        musicSource.Play();      //вкл аудио
    }

    // SFX
    public void PlaySFX(AudioClip clip) 
    {
        if (!clip) return;  // Проверка на null

        sfxSource.PlayOneShot(clip);  // Воспроизводит клип без остановки текущего звука
    }

    // Метод для регулировки громкости музыки
    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;  // value: 0.0 (тихо) — 1.0 (громко)
    }

    // Метод для регулировки громкости звуковых эффектов
    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;   // value: 0.0 (тихо) — 1.0 (громко)
    }
}
