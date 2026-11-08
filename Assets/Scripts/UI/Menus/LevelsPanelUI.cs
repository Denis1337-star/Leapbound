using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using YG;
using Zenject;

public sealed class LevelsPanelUI : MonoBehaviour
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
        _sceneFlowService.LoadLevelByBuildIndex(levelIndex);
    }
    private void RefreshLevelButtonsView()
    {
        if ( _levelButtons == null)
            return;

        for (int i = 0; i < _levelButtons.Length; i++)
        {
            LevelButton levelButton = _levelButtons[i];
            if (levelButton == null || levelButton.Button == null)
                continue;

            bool unlocked = _levelResultManager.IsLevelUnlocked(levelButton.LevelIndex);
            levelButton.Button.interactable = unlocked;

            if (levelButton.Stars == null)
                continue;

            int starCount = 0;
            if (unlocked)
                starCount = _levelResultManager.GetStarsForLevel(levelButton.LevelIndex);

            for (int starIndex = 0; starIndex < levelButton.Stars.Length; starIndex++)
            {
                if (levelButton.Stars[starIndex] == null)
                    continue;

                levelButton.Stars[starIndex].enabled = starIndex < starCount;
            }
        }
    }
}