using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

public sealed class GuidePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreGoalLabelText;
    [SerializeField] private Button _bonusHeartButton;
    private LevelStarCalculator _levelStarCalculator;
    private PlayerHealth _playerHealth;


    [Inject]
    public void Construct(LevelStarCalculator levelStarCalculator, PlayerHealth playerHealth)
    {
        _levelStarCalculator = levelStarCalculator;
        _playerHealth = playerHealth;
    }
    private void Awake()
    {
        _bonusHeartButton.onClick.AddListener(OnBonusHeartClicked);
    }
    private void OnDestroy()
    {
        _bonusHeartButton.onClick.RemoveListener(OnBonusHeartClicked);
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
        _scoreGoalLabelText.SetText("Очки: {0}", _levelStarCalculator.TargetScore);
    }
}