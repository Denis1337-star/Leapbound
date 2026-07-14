using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuUI : MonoBehaviour
{
    [System.Serializable]
    public class LevelButton
    {
        public string levelName;  
        public int levelIndex; 
        public Button button;  
        public Image[] stars;  
    }

    [SerializeField] private LevelButton[] _levelButtons;  
    [SerializeField] private Button _playButton;  
    [SerializeField] private Button _settingsButton;  
    [SerializeField] private GameObject _settingsPanel;

    private ISceneFlowService _sceneFlowService;

    [Inject]
    public void Construct(ISceneFlowService sceneFlowService)
    {
        _sceneFlowService = sceneFlowService;
    }

    private void Start()
    {
        _playButton.onClick.AddListener(PlayLastLevel); 
        _settingsButton.onClick.AddListener(() => _settingsPanel.SetActive(true));

        UpdateLevelButtons();  
    }

    private void UpdateLevelButtons()
    {
        foreach (var lvl in _levelButtons)  
        {
            bool unlocked = LevelResultManager.IsLevelUnlocked(lvl.levelIndex);
            lvl.button.interactable = unlocked;  

            int count = LevelResultManager.GetStarsForLevel(lvl.levelName);  
            for (int i = 0; i < lvl.stars.Length; i++) 
            {
                lvl.stars[i].enabled = i < count; 
            }

            lvl.button.onClick.RemoveAllListeners();   
            if (unlocked)  
            {
                string sceneName = lvl.levelName;

                lvl.button.onClick.AddListener(() => _sceneFlowService.LoadLevelByBuildIndex(lvl.levelIndex));  
            }
        }
    }

    private void PlayLastLevel()
    {
        int index = LevelResultManager.GetLastUnlockedLevel();

        foreach (var lvl in _levelButtons)
        {
            if (lvl.levelIndex == index)
            {
                _sceneFlowService.LoadLevelByBuildIndex(lvl.levelIndex);
                return;
            }
        }
    }
}
