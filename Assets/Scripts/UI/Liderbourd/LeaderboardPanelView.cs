using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using YG.Utils.LB;
using Zenject;

public sealed class LeaderboardPanelView : MonoBehaviour
{
    public enum Tab { Time, Crowns, Stars }

    [Header("Tabs")]
    [SerializeField] private Button _tabTimeButton;
    [SerializeField] private Button _tabCrownsButton;
    [SerializeField] private Button _tabStarsButton;
    [SerializeField] private Color _tabOn = new Color(0.96f, 0.93f, 0.86f);
    [SerializeField] private Color _tabOff = new Color(0.75f, 0.72f, 0.68f);

    [Header("Header / Levels")]
    [SerializeField] private TMP_Text _valueHeaderText;
    [SerializeField] private GameObject _levelStripRoot;
    [SerializeField] private LevelPillView _levelPillPrefab;
    [SerializeField] private Color _pillOn = new Color(0.35f, 0.82f, 0.38f);
    [SerializeField] private Color _pillOff = Color.white;

    [Header("Rows")]
    [SerializeField] private LeaderboardEntryView[] _topEntries;
    [SerializeField] private LeaderboardEntryView _myEntry;
    [SerializeField] private Button _backButton;

    private LevelTimeCatalog _catalog;
    private LevelPillView[] _pills;
    private Tab _tab;
    private int _level = 1;
    private string _requestedId;
    private float _nextRequestTime;

    public Button BackButton => _backButton;

    [Inject]
    public void Construct(LevelTimeCatalog catalog) { _catalog = catalog; }

    private void Awake()
    {
        _tabTimeButton.onClick.AddListener(() => SelectTab(Tab.Time));
        _tabCrownsButton.onClick.AddListener(() => SelectTab(Tab.Crowns));
        _tabStarsButton.onClick.AddListener(() => SelectTab(Tab.Stars));
        BuildPills();

        _tab = Tab.Time;
        SetTabBg(_tabTimeButton.image, true);
        SetTabBg(_tabCrownsButton.image, false);
        SetTabBg(_tabStarsButton.image, false);
        _valueHeaderText.SetText("Время");
        _levelStripRoot.SetActive(true);
    }
    private void OnEnable()
    {
        YG2.onGetLeaderboard += OnGetLeaderboard;
        RequestCurrentBoard();
    }
    private void OnDisable()
    {
        YG2.onGetLeaderboard -= OnGetLeaderboard;
    }
    private void OnDestroy()
    {
        _tabTimeButton.onClick.RemoveAllListeners();
        _tabCrownsButton.onClick.RemoveAllListeners();
        _tabStarsButton.onClick.RemoveAllListeners();
    }
    private void BuildPills()
    {
        int count = Mathf.Max(0, _catalog.LevelCount - 1);
        _pills = new LevelPillView[count];

        for (int i = 0; i < count; i++)
        {
            int level = i + 1;
            LevelPillView pill = Instantiate(_levelPillPrefab, _levelStripRoot.transform);
            pill.Setup(level, () => SelectLevel(level));
            _pills[i] = pill;
        }
        _level = 1;
        for (int i = 0; i < _pills.Length; i++)
            _pills[i].SetSelected(_pills[i].LevelIndex == 1, _pillOn, Color.white);
    }
    private void SelectTab(Tab tab)
    {
        _tab = tab;
        SetTabBg(_tabTimeButton.image, tab == Tab.Time);
        SetTabBg(_tabCrownsButton.image, tab == Tab.Crowns);
        SetTabBg(_tabStarsButton.image, tab == Tab.Stars);

        if (tab == Tab.Time)
            _valueHeaderText.SetText("Время");
        else if (tab == Tab.Crowns)
            _valueHeaderText.SetText("Короны");
        else
            _valueHeaderText.SetText("Звёзды");

        _levelStripRoot.SetActive(tab == Tab.Time);
        RequestCurrentBoard();
    }
    private void SelectLevel(int level)
    {
        _level = level;
        for (int i = 0; i < _pills.Length; i++)
            _pills[i].SetSelected(_pills[i].LevelIndex == level, _pillOn, _pillOff);

        RequestCurrentBoard();
    }
    private void SetTabBg(Image bg, bool on)
    {
        if (bg == null)
            return;

        if (on)
            bg.color = _tabOn;
        else
            bg.color = _tabOff;
    }
    private void RequestCurrentBoard()
    {
        if (!isActiveAndEnabled)
            return;

        if (Time.unscaledTime < _nextRequestTime)
            return;

        _nextRequestTime = Time.unscaledTime + 1.1f;
        if (_tab == Tab.Time)
            _requestedId = LeaderboardIds.Time(_level);
        else if (_tab == Tab.Crowns)
            _requestedId = LeaderboardIds.Crowns;
        else
            _requestedId = LeaderboardIds.Stars;
        YG2.GetLeaderboard(_requestedId, _topEntries.Length, 3);
    }
    private void OnGetLeaderboard(LBData data)
    {
        if (data == null || data.technoName != _requestedId)
            return;

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

        for (int i = 0; i < _topEntries.Length; i++)
        {
            if (players == null || i >= players.Length)
            {
                _topEntries[i].Set("—", "—", false);
                continue;
            }

            LBPlayerData p = players[i];
            bool isMe = !string.IsNullOrEmpty(myId) && p.uniqueID == myId;
            string name = isMe ? "Я" : LBMethods.AnonymousName(p.name);
            _topEntries[i].Set(name, FormatScore(p.score), isMe);
        }

        bool inTop = foundMe && myRank > 0 && myRank <= _topEntries.Length;
        _myEntry.gameObject.SetActive(!inTop && foundMe);

        if (!inTop && foundMe)
            _myEntry.SetMyPlace(myRank, FormatScore(myScore));
    }
    private string FormatScore(int score)
    {
        if (_tab == Tab.Time)
            return LBMethods.TimeTypeConvertStatic(score, 0);

        return score.ToString();
    }
}