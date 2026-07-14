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

        if (_musicSlider == null || _sfxSlider == null)
        {
            Debug.LogError($"{name} Sliders is miss");
            return;
        }

        _musicSlider.SetValueWithoutNotify(_settingsService.GetMusicValue());
        _sfxSlider.SetValueWithoutNotify(_settingsService.GetSFXValue());

        _musicSlider.onValueChanged.AddListener(OnMusicChanged);
        _sfxSlider.onValueChanged.AddListener(OnSfxChanged);

        _isBound = true;
    }
    private void OnDisable()
    {
        if (_musicSlider != null)
            _musicSlider.onValueChanged.RemoveListener(OnMusicChanged);

        if (_sfxSlider != null)
            _sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);

        _isBound = false;
    }
    private void OnMusicChanged(float value)
    {
        _settingsService.SetMusicValue(value);
    }
    private void OnSfxChanged(float value)
    {
        _settingsService.SetSFXValue(value);
    }
}
