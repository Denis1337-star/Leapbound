using UnityEngine;
using UnityEngine.UI;
using YG;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _levelsButton;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _levelsPanel;
    [SerializeField] private LevelsPanelUI _levelsPanelUI;
 private bool _reportGameReadyWhenMenuShown = true;
    private bool _hasReportedGameReady;

    private void Start()
    {
        _playButton.onClick.AddListener(OnPlayClicked);
        _settingsButton.onClick.AddListener(OpenSettings);
        _levelsButton.onClick.AddListener(OpenLevels);

        ReportGameReadyIfNeeded();
    }
    private void OnDestroy()
    {
        _playButton.onClick.RemoveListener(OnPlayClicked);
        _settingsButton.onClick.RemoveListener(OpenSettings);
        _levelsButton.onClick.RemoveListener(OpenLevels);
    }
    private void OnPlayClicked()
    {
        _levelsPanelUI.PlayLastUnlockedLevel();
    }
    private void OpenSettings()
    {
        _levelsPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }
    private void OpenLevels()
    {
        _settingsPanel.SetActive(false);
        _levelsPanel.SetActive(true);
    }
    private void ReportGameReadyIfNeeded()
    {
        if (!_reportGameReadyWhenMenuShown || _hasReportedGameReady)
            return;

        YG2.GameReadyAPI();
        _hasReportedGameReady = true;
    }
}