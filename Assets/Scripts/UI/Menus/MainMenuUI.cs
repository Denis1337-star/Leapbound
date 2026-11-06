using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using YG;
using Zenject;

public class MainMenuUI : MonoBehaviour
{
    [System.Serializable]
    public class LevelButton
    {
        [SerializeField, FormerlySerializedAs("levelIndex")]
        private int _levelIndex;
        [SerializeField, FormerlySerializedAs("button")]
        private Button _button;
        [SerializeField, FormerlySerializedAs("star")]
        private Image[] _stars;
        public int LevelIndex => _levelIndex;
        public Button Button => _button;
        public Image[] Stars => _stars;
    }

    [SerializeField] private LevelButton[] _levelButtons;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private GameObject _settingsPanel;
    private bool _reportGameReadyWhenMenuShown = true; //слать ли площадке?
    private bool _hasReportedGameReady; //отправил Ready?
    private ISceneFlowService _sceneFlowService;
    private LevelResultManager _levelResultManager;

    [Inject]
    public void Construct(ISceneFlowService sceneFlowService, LevelResultManager levelResultManager)
    {
        _sceneFlowService = sceneFlowService;
        _levelResultManager = levelResultManager;
    }

    private void OnEnable()
    {
        YG2.onGetSDKData += RefreshLevelButtonsView;
    }
    private void OnDisable()
    {
        YG2.onGetSDKData -= RefreshLevelButtonsView;
    }
    private void Start()
    {
        _playButton.onClick.AddListener(PlayLastLevel);
        _settingsButton.onClick.AddListener(OpenSetting);

        BindLevelButtonClicks();   
        RefreshLevelButtonsView(); 
        ReportGameReadyIfNeeded();
    }
    private void OnDestroy()
    {
        _playButton.onClick.RemoveListener(PlayLastLevel);
        _settingsButton.onClick.RemoveListener(OpenSetting);
    }
    private void OpenSetting() { _settingsPanel.SetActive(true); }
    
    private void PlayLastLevel()
    {
        int index = _levelResultManager.GetLastUnlockedLevel();

        for (int i = 0; i < _levelButtons.Length; i++)
        {
            if (_levelButtons[i].LevelIndex != index)
                continue;

            _sceneFlowService.LoadLevelByBuildIndex(index);
            return;
        }
    }
    private void LoadLevel(int levelIndex)
    {
        _sceneFlowService.LoadLevelByBuildIndex(levelIndex);
    }
    private void ReportGameReadyIfNeeded()
    {
        if (!_reportGameReadyWhenMenuShown || _hasReportedGameReady)
            return;

        YG2.GameReadyAPI();
        _hasReportedGameReady = true;
    }
    private void RefreshLevelButtonsView()
    {
        for (int i = 0; i < _levelButtons.Length; i++)
        {
            LevelButton levelButton = _levelButtons[i];
            bool unlocked = _levelResultManager.IsLevelUnlocked(levelButton.LevelIndex);
            levelButton.Button.interactable = unlocked;

            int starCount = _levelResultManager.GetStarsForLevel(levelButton.LevelIndex);
            for (int starIndex = 0; starIndex < levelButton.Stars.Length; starIndex++)
                levelButton.Stars[starIndex].enabled = starIndex < starCount;
        }
    }
    private void BindLevelButtonClicks()
    {
        for (int i = 0; i < _levelButtons.Length; i++)
        {
            LevelButton levelButton = _levelButtons[i];
            int levelIndex = levelButton.LevelIndex;
            levelButton.Button.onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }
}
