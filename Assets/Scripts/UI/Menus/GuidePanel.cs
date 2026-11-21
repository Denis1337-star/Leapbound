using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
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
    }
    private void OnDestroy()
    {
        _bonusHeartButton.onClick.RemoveListener(OnBonusHeartClicked);
        _okButton.onClick.RemoveListener(OnOkClicked);
    }
    private void OnBonusHeartClicked()
    {
        if (_playerHealth.HasBonusMaxHealth)
            return;
        YG2.RewardedAdvShow("extra_heart", GrantBonusHeartAfterReward);
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

            _timeTrialTargetText.SetText(string.Format("Уложись в {0}",
                LevelTimeFormat.Format(_levelTimer.TargetSeconds)));

            int buildIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            if (_levelResultManager.HasBestTime(buildIndex))
                _timeTrialBestText.SetText(string.Format("Твой рекорд: {0}",
                    LevelTimeFormat.Format(_levelResultManager.GetBestTime(buildIndex))));
            else
                _timeTrialBestText.SetText("Рекорда ещё нет");
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
}