using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public Slider musicSlider;  //слайдер по настройку громкости 
    public Slider sfxSlider;

    private void Start()
    {
        if (SettingsManager.Instance == null) return;  //проверка на менеджер настроект

        // Установить текущие значения
        musicSlider.value = SettingsManager.Instance.GetMusicValue();  //загружает положение ползунков 
        sfxSlider.value = SettingsManager.Instance.GetSFXValue();

        // Подписка на изменения
        musicSlider.onValueChanged.AddListener(v => SettingsManager.Instance.SetMusicValue(v));  //при перемещнеи ползунка меняет громкость
        sfxSlider.onValueChanged.AddListener(v => SettingsManager.Instance.SetSFXValue(v));
    }
}
