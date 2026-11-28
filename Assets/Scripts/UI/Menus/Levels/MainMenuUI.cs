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
    [SerializeField] private Button _heroesButton;
    [SerializeField] private HeroesPanelView _heroesPanel;
    [SerializeField] private Button _aboutButton;
    [SerializeField] private GameObject _aboutPanel;
    [SerializeField] private Button _aboutBackButton;
    [SerializeField] private Button _otherGameButton;
    private bool _reportGameReadyWhenMenuShown = true;
    private bool _hasReportedGameReady;
    private LevelRunContext _levelRunContext;
    private HeroesPanelLogic _heroesPanelLogic;
    private HeroProgress _heroProgress;
    private AnalyticsService _analyticsService;
    private const string OtherGameUrl = "https://yandex.ru/games/app/539279";

    [Inject]
    public void Construct(LevelRunContext levelRunContext, HeroProgress heroProgress, AnalyticsService analyticsService)
    {
        _levelRunContext = levelRunContext;
        _heroProgress = heroProgress;
        _analyticsService = analyticsService;
        _heroesPanelLogic = new HeroesPanelLogic(
           _heroesPanel, levelRunContext, heroProgress, RestoreMenuAfterHeroes, analyticsService);
    }

    private void Start()
    {
        _heroProgress.ApplySaved(_levelRunContext);

        _playButton.onClick.AddListener(OnPlayClicked);
        _settingsButton.onClick.AddListener(OpenSettings);
        _levelsButton.onClick.AddListener(OpenLevels);
        _leaderboardButton.onClick.AddListener(OpenLeaderboard);
        _leaderboardPanel.BackButton.onClick.AddListener(CloseLeaderboard);
        _heroesButton.onClick.AddListener(OpenHeroes);
        _aboutButton.onClick.AddListener(OpenAbout);
        _aboutBackButton.onClick.AddListener(CloseAbout);
        _otherGameButton.onClick.AddListener(OpenOtherGame);

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
        _heroesButton.onClick.RemoveListener(OpenHeroes);
        _aboutButton.onClick.RemoveListener(OpenAbout);
        _aboutBackButton.onClick.RemoveListener(CloseAbout);
        _otherGameButton.onClick.RemoveListener(OpenOtherGame);

        if (_heroesPanelLogic != null)
        {
            _heroesPanelLogic.Dispose();
            _heroesPanelLogic = null;
        }
    }
    private void OnPlayClicked()
    {
        if (_levelsPanelUI.HasCompletedAllLevelsWithStars())
        {
            OpenLevels();
            return;
        }

        _levelsPanelUI.PlayLastUnlockedLevel();
    }
    private void OpenSettings()
    {
        _menuRoot.SetActive(false);
        _levelsPanel.SetActive(false);
        _settingsPanel.SetActive(true);
        _leaderboardPanel.gameObject.SetActive(false);
        _heroesPanel.Close();
        _aboutPanel.SetActive(false);
    }
    private void OpenLevels()
    {
        _menuRoot.SetActive(false);
        _settingsPanel.SetActive(false);
        _levelsPanel.SetActive(true);
        _leaderboardPanel.gameObject.SetActive(false);
        _heroesPanel.Close();
        _aboutPanel.SetActive(false);
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
        _analyticsService.LeaderboardOpen();
        _menuRoot.SetActive(false);
        _settingsPanel.SetActive(false);
        _levelsPanel.SetActive(false);
        _leaderboardPanel.gameObject.SetActive(true);
        _heroesPanel.Close();
        _aboutPanel.SetActive(false);
    }

    private void CloseLeaderboard()
    {
        _leaderboardPanel.gameObject.SetActive(false);
        _menuRoot.SetActive(true);
    }
    private void OpenHeroes()
    {
        _menuRoot.SetActive(false);
        _aboutPanel.SetActive(false);
        _settingsPanel.SetActive(false);
        _levelsPanel.SetActive(false);
        _leaderboardPanel.gameObject.SetActive(false);
        _heroesPanelLogic.Open();
    }
    private void RestoreMenuAfterHeroes() { _menuRoot.SetActive(true); }
    private void OpenAbout()
    {
        _analyticsService.AboutOpen();
        _menuRoot.SetActive(false);
        _settingsPanel.SetActive(false);
        _levelsPanel.SetActive(false);
        _leaderboardPanel.gameObject.SetActive(false);
        _heroesPanel.Close();
        _aboutPanel.SetActive(true);
    }

    private void CloseAbout()
    {
        _aboutPanel.SetActive(false);
        _menuRoot.SetActive(true);
    }

    private void OpenOtherGame()
    {
        YG2.OnURL(OtherGameUrl);
    }
}
