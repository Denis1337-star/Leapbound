using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AudioSettingsPanel : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private ISettingsService _settingsService;
    private bool _isBound;

    [Inject]
    public void Construct(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    private void OnEnable()
    {
        BindSliders();
    }

    private void BindSliders()
    {
        if (_isBound) return;

        _musicSlider.SetValueWithoutNotify(_settingsService.MusicVolume);
        _sfxSlider.SetValueWithoutNotify(_settingsService.SfxVolume);

        _musicSlider.onValueChanged.AddListener(OnMusicChanged);
        _sfxSlider.onValueChanged.AddListener(OnSfxChanged);

        _isBound = true;
    }
    private void OnDisable()
    {
        _musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
        _sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);

        _isBound = false;
    }
    private void OnMusicChanged(float value) { _settingsService.SetMusicVolume(value); }
    private void OnSfxChanged(float value) { _settingsService.SetSfxVolume(value); }
}
