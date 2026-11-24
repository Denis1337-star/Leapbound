using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _levelsButton;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _levelsPanel;
    [SerializeField] private LevelsPanelUI _levelsPanelUI;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private LeaderboardPanelView _leaderboardPanel;
    [SerializeField] private GameObject _menuRoot;
    private bool _reportGameReadyWhenMenuShown = true;
    private bool _hasReportedGameReady;
    private LevelRunContext _levelRunContext;

    [Inject]
    public void Construct(LevelRunContext levelRunContext)
    {
        _levelRunContext = levelRunContext;
    }

    private void Start()
    {
        _playButton.onClick.AddListener(OnPlayClicked);
        _settingsButton.onClick.AddListener(OpenSettings);
        _levelsButton.onClick.AddListener(OpenLevels);
        _leaderboardButton.onClick.AddListener(OpenLeaderboard);
        _leaderboardPanel.BackButton.onClick.AddListener(CloseLeaderboard);

        ReportGameReadyIfNeeded();
        if (_levelRunContext != null && _levelRunContext.ConsumeOpenLevelsOnMainMenu())
            OpenLevels();
    }
    private void OnDestroy()
    {
        _playButton.onClick.RemoveListener(OnPlayClicked);
        _settingsButton.onClick.RemoveListener(OpenSettings);
        _levelsButton.onClick.RemoveListener(OpenLevels);
        _leaderboardButton.onClick.RemoveListener(OpenLeaderboard);
        _leaderboardPanel.BackButton.onClick.RemoveListener(CloseLeaderboard);
    }
    private void OnPlayClicked()
    {
        _levelsPanelUI.PlayLastUnlockedLevel();
    }
    private void OpenSettings()
    {
        _levelsPanel.SetActive(false);
        _settingsPanel.SetActive(true);
        _leaderboardPanel.gameObject.SetActive(false);
    }
    private void OpenLevels()
    {
        _settingsPanel.SetActive(false);
        _levelsPanel.SetActive(true);
        _leaderboardPanel.gameObject.SetActive(false);
    }
    private void ReportGameReadyIfNeeded()
    {
        if (!_reportGameReadyWhenMenuShown || _hasReportedGameReady)
            return;

        YG2.GameReadyAPI();
        _hasReportedGameReady = true;
    }
    private void OpenLeaderboard()
    {
        if (_menuRoot != null)
            _menuRoot.SetActive(false);

        _settingsPanel.SetActive(false);
        _levelsPanel.SetActive(false);

        if (_leaderboardPanel != null)
            _leaderboardPanel.gameObject.SetActive(true);
    }

    private void CloseLeaderboard()
    {
        if (_leaderboardPanel != null)
            _leaderboardPanel.gameObject.SetActive(false);

        if (_menuRoot != null)
            _menuRoot.SetActive(true);
    }
}
