using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

public sealed class LevelsPanelUI : MonoBehaviour
{
    [System.Serializable]
    public class LevelButton
    {
        [SerializeField] private int _levelIndex;
        [SerializeField] private Button _button;
        [SerializeField] private Image[] _stars;
        [SerializeField] private Image _lockIcon;
        [SerializeField] private Image _crownIcon;

        public Image CrownIcon => _crownIcon;
        public Image LockIcon => _lockIcon;
        public int LevelIndex => _levelIndex;
        public Button Button => _button;
        public Image[] Stars => _stars;
    }

    [SerializeField] private LevelButton[] _levelButtons;
    [SerializeField] private LevelModeChoicePanel _levelModeChoicePanel;

    private ISceneFlowService _sceneFlowService;
    private LevelResultManager _levelResultManager;
    private LevelRunContext _levelRunContext;
    private LevelTimeCatalog _levelTimeCatalog;


    [Inject]
    public void Construct(ISceneFlowService sceneFlowService, LevelResultManager levelResultManager,
        LevelRunContext levelRunContext, LevelTimeCatalog levelTimeCatalog)
    {
        _sceneFlowService = sceneFlowService;
        _levelResultManager = levelResultManager;
        _levelRunContext = levelRunContext;
        _levelTimeCatalog = levelTimeCatalog;
    }
    private void OnEnable()
    {
        YG2.onGetSDKData += RefreshLevelButtonsView;
        RefreshLevelButtonsView();
    }
    private void OnDisable()
    {
        YG2.onGetSDKData -= RefreshLevelButtonsView;
    }
    private void Start()
    {
        BindLevelButtonClicks();
        RefreshLevelButtonsView();
    }
    public void PlayLastUnlockedLevel()
    {
        if ( _levelButtons == null)
            return;

        int index = _levelResultManager.GetLastUnlockedLevel();
        for (int i = 0; i < _levelButtons.Length; i++)
        {
            LevelButton levelButton = _levelButtons[i];
            if (levelButton == null || levelButton.LevelIndex != index)
                continue;

            _levelRunContext.SetNormal();
            _sceneFlowService.LoadLevelByBuildIndex(index);
            return;
        }
    }
    private void BindLevelButtonClicks()
    {
        if (_levelButtons == null)
            return;

        for (int i = 0; i < _levelButtons.Length; i++)
        {
            LevelButton levelButton = _levelButtons[i];
            if (levelButton == null || levelButton.Button == null)
                continue;

            int levelIndex = levelButton.LevelIndex;
            levelButton.Button.onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }
    private void LoadLevel(int levelIndex)
    {
        LevelButton levelButton = FindLevelButton(levelIndex);
        if (levelButton == null)
            return;

        if (_levelResultManager.CanStartTimeTrial(levelIndex))
        {
            _levelModeChoicePanel.Open(
                levelIndex,
           _levelTimeCatalog.GetTargetTimeSeconds(levelIndex),
                _levelResultManager.GetBestTime(levelIndex),
                _levelResultManager.HasBestTime(levelIndex));
            return;
        }

        _levelRunContext.SetNormal();
        _sceneFlowService.LoadLevelByBuildIndex(levelIndex);
    }
    private void RefreshLevelButtonsView()
    {
        if (_levelButtons == null)
            return;

        for (int i = 0; i < _levelButtons.Length; i++)
        {
            LevelButton levelButton = _levelButtons[i];
            if (levelButton == null || levelButton.Button == null)
                continue;

            bool unlocked = _levelResultManager.IsLevelUnlocked(levelButton.LevelIndex);
            levelButton.Button.interactable = unlocked;

            if (levelButton.LockIcon != null)
                levelButton.LockIcon.gameObject.SetActive(!unlocked);

            if (levelButton.Stars == null)
                continue;

            int starCount = 0;
            if (unlocked)
                starCount = _levelResultManager.GetStarsForLevel(levelButton.LevelIndex);

            for (int starIndex = 0; starIndex < levelButton.Stars.Length; starIndex++)
            {
                if (levelButton.Stars[starIndex] == null)
                    continue;

                levelButton.Stars[starIndex].enabled = unlocked && starIndex < starCount;
            }

            if (levelButton.CrownIcon != null)
                levelButton.CrownIcon.gameObject.SetActive(
                    unlocked && _levelResultManager.HasCrown(levelButton.LevelIndex));
        }

    }
    private LevelButton FindLevelButton(int levelIndex)
    {
        for (int i = 0; i < _levelButtons.Length; i++)
        {
            if (_levelButtons[i] != null && _levelButtons[i].LevelIndex == levelIndex)
                return _levelButtons[i];
        }
        return null;
    }
}