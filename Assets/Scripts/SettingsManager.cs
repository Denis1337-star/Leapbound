using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;  //одмн экземпляр на всю игру

    public AudioMixer audioMixer; //управляет группами аудио

    private const string MUSIC_KEY = "MusicVolume";  //ключ для сохранение громкости
    private const string SFX_KEY = "SFXVolume";

    private float musicValue = 1f;  //текущий уровень громкости
    private float sfxValue = 1f;

    private void Awake()
    {
        if (Instance == null) //текущий обьект единственый
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //сохраняем обьект между сценами 
        }
        else
        {
            Destroy(gameObject); //если есть убирает
            return;
        }
    }

    private void Start()
    {
        LoadValues();//загружает сохраненные значения громкости
        ApplyVolumes();  //применяет их
    }

    private void LoadValues()
    {
        musicValue = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);  //из ключей берет данные если нет то =1
        sfxValue = PlayerPrefs.GetFloat(SFX_KEY, 1f);
    }

    public float GetMusicValue() => musicValue;  //возращает текущий уровень громкости
    public float GetSFXValue() => sfxValue;

    public void SetMusicValue(float value)
    {
        musicValue = value;  //сохр новое значение 
        PlayerPrefs.SetFloat(MUSIC_KEY, value);  //записывает в ключ
        ApplyVolumes();   //применяет
    }

    public void SetSFXValue(float value)
    {
        sfxValue = value;
        PlayerPrefs.SetFloat(SFX_KEY, value);
        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        SetVolume("MusicVolume", musicValue);  //применяет для микшеров 
        SetVolume("SFXVolume", sfxValue);
    }

    private void SetVolume(string name, float value)
    {
        float db = value <= 0.0001f ? -80f : Mathf.Log10(value) * 20f;  //перевод в ДЦ 
        audioMixer.SetFloat(name, db); //применяет в микшер
    }
}

