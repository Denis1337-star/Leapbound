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


    [Inject]
    public void Construct(LevelStarCalculator levelStarCalculator, PlayerHealth playerHealth,
        LevelRunContext levelRunContext, LevelTimer levelTimer, LevelResultManager levelResultManager)
    {
        _levelStarCalculator = levelStarCalculator;
        _playerHealth = playerHealth;
        _levelRunContext = levelRunContext;
        _levelTimer = levelTimer;
        _levelResultManager = levelResultManager;
    }
    private void Awake()
    {
        _bonusHeartButton.onClick.AddListener(OnBonusHeartClicked);
        _okButton.onClick.AddListener(OnOkClicked);
        _bonusTimeButton.onClick.AddListener(OnBonusTimeClicked);
    }
    private void OnDestroy()
    {
        _bonusHeartButton.onClick.RemoveListener(OnBonusHeartClicked);
        _okButton.onClick.RemoveListener(OnOkClicked);
        _bonusTimeButton.onClick.RemoveListener(OnBonusTimeClicked);
    }
    private void OnBonusHeartClicked()
    {
        if (_playerHealth.HasBonusMaxHealth)
            return;
        YandexRewardedBridge.Show("extra_heart", GrantBonusHeartAfterReward);
    }
    private void GrantBonusHeartAfterReward()
    {
        _playerHealth.GrantBonusMaxHealth();
        _bonusHeartButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (_levelRunContext.Mode == LevelRunMode.TimeTrial)
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

        if (_levelRunContext.Mode == LevelRunMode.TimeTrial)
            _levelTimer.StartTimer();

        gameObject.SetActive(false);
    }
    private void OnBonusTimeClicked()
    {
        if (_levelRunContext.Mode != LevelRunMode.TimeTrial)
            return;
        if (_levelTimer.HasBonusTime)
            return;

        YandexRewardedBridge.Show("extra_time", GrantBonusTimeAfterReward);
    }

    private void GrantBonusTimeAfterReward()
    {
        _levelTimer.ExtendTarget(_bonusTimeSeconds);
        _timeTrialTargetText.SetText(LevelTimeLabels.TargetGuide(_levelTimer.TargetSeconds));

        _bonusTimeButton.gameObject.SetActive(false);
    }
}