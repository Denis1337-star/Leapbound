using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public sealed class GuidePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreGoalLabelText;
    [SerializeField] private Button _bonusHeartButton;
    [SerializeField] private Button _okButton;
    [SerializeField] private GameObject _normalGuideRoot;
    [SerializeField] private GameObject _timeTrialGuideRoot;
    [SerializeField] private TMP_Text _timeTrialTargetText;
    [SerializeField] private TMP_Text _timeTrialBestText;
    [SerializeField] private Button _bonusTimeButton;
    private float _bonusTimeSeconds = 20f;
    private LevelStarCalculator _levelStarCalculator;
    private PlayerHealth _playerHealth;
    private LevelRunContext _levelRunContext;
    private LevelTimer _levelTimer;
    private LevelResultManager _levelResultManager;
    private GuidePanelAds _guidePanelAds;
    [Inject]
    public void Construct(LevelStarCalculator levelStarCalculator,PlayerHealth playerHealth,
        LevelRunContext levelRunContext,LevelTimer levelTimer,LevelResultManager levelResultManager)
    {
        _levelStarCalculator = levelStarCalculator;
        _playerHealth = playerHealth;
        _levelRunContext = levelRunContext;
        _levelTimer = levelTimer;
        _levelResultManager = levelResultManager;
    }
    private void Awake()
    {
        _guidePanelAds = new GuidePanelAds(_playerHealth,_levelRunContext,_levelTimer,
            _bonusTimeSeconds,OnHeartGranted,OnTimeGranted);

        _bonusHeartButton.onClick.AddListener(_guidePanelAds.OnBonusHeartClicked);
        _bonusTimeButton.onClick.AddListener(_guidePanelAds.OnBonusTimeClicked);
        _okButton.onClick.AddListener(OnOkClicked);
    }
    private void OnDestroy()
    {
        _bonusHeartButton.onClick.RemoveListener(_guidePanelAds.OnBonusHeartClicked);
        _bonusTimeButton.onClick.RemoveListener(_guidePanelAds.OnBonusTimeClicked);
        _okButton.onClick.RemoveListener(OnOkClicked);
    }
    private void Start()
    {
        if (_levelRunContext.LevelMode == LevelRunMode.TimeTrial)
        {
            _normalGuideRoot.SetActive(false);
            _timeTrialGuideRoot.SetActive(true);
            _timeTrialTargetText.SetText(LevelTimeLabels.TargetGuide(_levelTimer.TargetSeconds));

            int buildIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            if (_levelResultManager.HasBestTime(buildIndex))
                _timeTrialBestText.SetText(LevelTimeLabels.BestGuide(_levelResultManager.GetBestTime(buildIndex)));
            else
                _timeTrialBestText.SetText(LevelTimeLabels.NoRecord);

            _bonusTimeButton.gameObject.SetActive(!_levelTimer.HasBonusTime);
        }
        else
        {
            _normalGuideRoot.SetActive(true);
            _timeTrialGuideRoot.SetActive(false);
            _scoreGoalLabelText.SetText("Очки: {0}", _levelStarCalculator.TargetScore);
        }
        _levelRunContext.SetInputLocked(true);
    }
    private void OnOkClicked()
    {
        _levelRunContext.SetInputLocked(false);
        if (_levelRunContext.LevelMode == LevelRunMode.TimeTrial)
            _levelTimer.StartTimer();

        gameObject.SetActive(false);
    }
    private void OnHeartGranted()
    {
        _bonusHeartButton.gameObject.SetActive(false);
    }
    private void OnTimeGranted()
    {
        _timeTrialTargetText.SetText(LevelTimeLabels.TargetGuide(_levelTimer.TargetSeconds));
        _bonusTimeButton.gameObject.SetActive(false);
    }
}