using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using YG.Utils.LB;
using Zenject;

public sealed class LeaderboardPanelView : MonoBehaviour
{

    [Header("Tabs")]
    [SerializeField] private Button _tabTimeButton;
    [SerializeField] private Button _tabCrownsButton;
    [SerializeField] private Button _tabStarsButton;
    [SerializeField] private Color _tabOff = new Color(0.75f, 0.72f, 0.68f);

    [Header("Header / Levels")]
    [SerializeField] private TMP_Text _valueHeaderText;
    [SerializeField] private GameObject _levelStripRoot;
    [SerializeField] private LevelPillView _levelPillPrefab;
    [SerializeField] private Color _selectedLevelPillColor = new Color(0.35f, 0.82f, 0.38f);

    [Header("Rows")]
    [SerializeField] private LeaderboardEntryView[] _topEntriesLeaderboard;
    [SerializeField] private LeaderboardEntryView _myEntryLeaderboard;
    [SerializeField] private Button _backButton;

    private LevelTimeCatalog _levelTimeCatalog;
    private LeaderboardPanelLogic _leaderboardPanelLogic;
    private LevelPillView[] _levelPills;
    public Button BackButton => _backButton;
    [Inject]
    public void Construct(LevelTimeCatalog levelTimeCatalog)
    {
        _levelTimeCatalog = levelTimeCatalog;
    }
    private void Awake()
    {
        _leaderboardPanelLogic = new LeaderboardPanelLogic(_topEntriesLeaderboard.Length, ApplyBoard);
        _tabTimeButton.onClick.AddListener(() => OnTabClicked(LeaderboardPanelLogic.Tab.Time));
        _tabCrownsButton.onClick.AddListener(() => OnTabClicked(LeaderboardPanelLogic.Tab.Crowns));
        _tabStarsButton.onClick.AddListener(() => OnTabClicked(LeaderboardPanelLogic.Tab.Stars));
        BuildPills();
        PaintTabs(LeaderboardPanelLogic.Tab.Time);
    }
    private void OnEnable() { _leaderboardPanelLogic.Enable(); }
    private void OnDisable() { _leaderboardPanelLogic.Disable(); }
    private void OnDestroy()
    {
        _tabTimeButton.onClick.RemoveAllListeners();
        _tabCrownsButton.onClick.RemoveAllListeners();
        _tabStarsButton.onClick.RemoveAllListeners();
    }
    private void OnTabClicked(LeaderboardPanelLogic.Tab tab)
    {
        PaintTabs(tab);
        _leaderboardPanelLogic.SelectTab(tab);
    }
    private void OnLevelClicked(int level)
    {
        PaintPills(level);
        _leaderboardPanelLogic.SelectLevel(level);
    }
    private void BuildPills()
    {
        int count = Mathf.Max(0, _levelTimeCatalog.LevelCount - 1);
        _levelPills = new LevelPillView[count];
        for (int i = 0; i < count; i++)
        {
            int level = i + 1;
            LevelPillView pill = Instantiate(_levelPillPrefab, _levelStripRoot.transform);
            pill.Setup(level, () => OnLevelClicked(level));
            _levelPills[i] = pill;
        }
        PaintPills(1);
    }
    private void PaintTabs(LeaderboardPanelLogic.Tab tab)
    {
        SetTabBg(_tabTimeButton.image, tab == LeaderboardPanelLogic.Tab.Time);
        SetTabBg(_tabCrownsButton.image, tab == LeaderboardPanelLogic.Tab.Crowns);
        SetTabBg(_tabStarsButton.image, tab == LeaderboardPanelLogic.Tab.Stars);

        if (tab == LeaderboardPanelLogic.Tab.Time)
            _valueHeaderText.SetText("Время");
        else if (tab == LeaderboardPanelLogic.Tab.Crowns)
            _valueHeaderText.SetText("Короны");
        else
            _valueHeaderText.SetText("Звёзды");

        _levelStripRoot.SetActive(tab == LeaderboardPanelLogic.Tab.Time);
    }
    private void PaintPills(int selectedLevel)
    {
        for (int i = 0; i < _levelPills.Length; i++)
            _levelPills[i].SetSelected(_levelPills[i].LevelIndex == selectedLevel, _selectedLevelPillColor, Color.white);
    }
    private void SetTabBg(Image bg, bool on)
    {
        if (bg == null)
            return;

        if (on)
            bg.color = Color.white;
        else
            bg.color = _tabOff;
    }
    private void ApplyBoard(LBData data)
    {
        LBPlayerData[] players = data.players;
        string myId = YG2.player.id;
        int myRank = 0;
        int myScore = 0;
        bool foundMe = false;

        if (players != null)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].uniqueID != myId)
                    continue;

                myRank = players[i].rank;
                myScore = players[i].score;
                foundMe = true;
                break;
            }
        }
        for (int i = 0; i < _topEntriesLeaderboard.Length; i++)
        {
            if (players == null || i >= players.Length)
            {
                _topEntriesLeaderboard[i].Set("—", "—", false);
                continue;
            }
            LBPlayerData p = players[i];
            bool isMe = !string.IsNullOrEmpty(myId) && p.uniqueID == myId;
            string name;
            if (isMe)
                name = "Я";
            else
                name = LBMethods.AnonymousName(p.name);

            _topEntriesLeaderboard[i].Set(name, FormatScore(p.score), isMe);
        }
        bool inTop = foundMe && myRank > 0 && myRank <= _topEntriesLeaderboard.Length;
        _myEntryLeaderboard.gameObject.SetActive(!inTop && foundMe);
        if (!inTop && foundMe)
            _myEntryLeaderboard.SetMyPlace(myRank, FormatScore(myScore));
    }
    private string FormatScore(int score)
    {
        if (_leaderboardPanelLogic.CurrentTab == LeaderboardPanelLogic.Tab.Time)
            return LBMethods.TimeTypeConvertStatic(score, 0);

        return score.ToString();
    }
}

