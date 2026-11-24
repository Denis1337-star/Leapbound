using System;

public sealed class GuidePanelAds
{
    private readonly PlayerHealth _playerHealth;
    private readonly LevelRunContext _levelRunContext;
    private readonly LevelTimer _levelTimer;
    private readonly float _bonusTimeSeconds;
    private readonly Action _onHeartGranted;
    private readonly Action _onTimeGranted;

    public GuidePanelAds(PlayerHealth playerHealth,LevelRunContext levelRunContext,
        LevelTimer levelTimer,float bonusTimeSeconds,Action onHeartGranted,Action onTimeGranted)
    {
        _playerHealth = playerHealth;
        _levelRunContext = levelRunContext;
        _levelTimer = levelTimer;
        _bonusTimeSeconds = bonusTimeSeconds;
        _onHeartGranted = onHeartGranted;
        _onTimeGranted = onTimeGranted;
    }

    public void OnBonusHeartClicked()
    {
        if (_playerHealth.HasBonusMaxHealth)
            return;

        YandexRewardedBridge.Show("extra_heart", GrantBonusHeartAfterReward);
    }

    public void OnBonusTimeClicked()
    {
        if (_levelRunContext.LevelMode != LevelRunMode.TimeTrial)
            return;

        if (_levelTimer.HasBonusTime)
            return;

        YandexRewardedBridge.Show("extra_time", GrantBonusTimeAfterReward);
    }

    private void GrantBonusHeartAfterReward()
    {
        _playerHealth.GrantBonusMaxHealth();
        _onHeartGranted?.Invoke();
    }

    private void GrantBonusTimeAfterReward()
    {
        _levelTimer.ExtendTarget(_bonusTimeSeconds);
        _onTimeGranted?.Invoke();
    }
}