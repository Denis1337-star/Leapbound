using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;  
    [SerializeField] private Slider _sfxSlider;

    private void Start()
    {
        if (SettingsManager.Instance == null) return;  //проверка на менеджер настроект

        // Установить текущие значения
        _musicSlider.value = SettingsManager.Instance.GetMusicValue();  //загружает положение ползунков 
        _sfxSlider.value = SettingsManager.Instance.GetSFXValue();

        // Подписка на изменения
        _musicSlider.onValueChanged.AddListener(v => SettingsManager.Instance.SetMusicValue(v));  //при перемещнеи ползунка меняет громкость
        _sfxSlider.onValueChanged.AddListener(v => SettingsManager.Instance.SetSFXValue(v));
    }
}
