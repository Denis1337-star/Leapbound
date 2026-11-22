using System;
using YG;

public static class YandexRewardedBridge
{
    public static void Show(string rewardId, Action onGranted)
    {
        YG2.RewardedAdvShow(rewardId, onGranted);
    }
}