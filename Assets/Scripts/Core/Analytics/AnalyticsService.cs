using System.Collections.Generic;
using YG;

public sealed class AnalyticsService
{
    public void LevelStart(int levelBuildIndex, string runMode, string selectedHero)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["level_build_index"] = levelBuildIndex.ToString();
        data["run_mode"] = runMode;
        data["selected_hero"] = selectedHero;
        SendEvent("level_start", data);
    }

    public void LevelComplete(int levelBuildIndex,string runMode,string selectedHero,
        int starsCount,int elapsedSeconds,int deathCount,bool finishedUnderTarget)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["level_build_index"] = levelBuildIndex.ToString();
        data["run_mode"] = runMode;
        data["selected_hero"] = selectedHero;
        data["stars_count"] = starsCount.ToString();
        data["elapsed_seconds"] = elapsedSeconds.ToString();
        data["death_count"] = deathCount.ToString();
        if (runMode == "time_trial")
        {
            if (finishedUnderTarget)
                data["finished_under_target"] = "1";
            else
                data["finished_under_target"] = "0";
        }
        SendEvent("level_complete", data);
    }

    public void LevelFail(int levelBuildIndex, string runMode, int deathCount, int elapsedSeconds)
    {
        Dictionary<string, string> data = CreateLevelBase(levelBuildIndex, runMode, deathCount, elapsedSeconds);
        SendEvent("level_fail", data);
    }

    public void LevelRestart(int levelBuildIndex, string runMode, int deathCount, int elapsedSeconds)
    {
        Dictionary<string, string> data = CreateLevelBase(levelBuildIndex, runMode, deathCount, elapsedSeconds);
        SendEvent("level_restart", data);
    }

    public void LevelQuit(int levelBuildIndex, string runMode, int deathCount, int elapsedSeconds)
    {
        Dictionary<string, string> data = CreateLevelBase(levelBuildIndex, runMode, deathCount, elapsedSeconds);
        SendEvent("level_quit", data);
    }

    public void AdInterstitialShow(string adPlacement)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["ad_placement"] = adPlacement;
        SendEvent("ad_interstitial_show", data);
    }

    public void AdInterstitialClose(string adPlacement)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["ad_placement"] = adPlacement;
        SendEvent("ad_interstitial_close", data);
    }

    public void AdRewardedShow(string rewardType)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["reward_type"] = rewardType;
        SendEvent("ad_rewarded_show", data);
    }

    public void AdRewardedReward(string rewardType)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["reward_type"] = rewardType;
        SendEvent("ad_rewarded_reward", data);
    }

    public void ExtraLifeTaken(int levelBuildIndex)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["level_build_index"] = levelBuildIndex.ToString();
        SendEvent("extra_life_taken", data);
    }

    public void HeroSelect(string selectedHero)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["selected_hero"] = selectedHero;
        SendEvent("hero_select", data);
    }


    private Dictionary<string, string> CreateLevelBase(int levelBuildIndex, string runMode, 
        int deathCount, int elapsedSeconds)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["level_build_index"] = levelBuildIndex.ToString();
        data["run_mode"] = runMode;
        data["death_count"] = deathCount.ToString();
        data["elapsed_seconds"] = elapsedSeconds.ToString();
        return data;
    }
    public void HeroTrialStart(string selectedHero)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["selected_hero"] = selectedHero;
        SendEvent("hero_trial_start", data);
    }

    public void HeroBuy(string selectedHero)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["selected_hero"] = selectedHero;
        SendEvent("hero_buy", data);
    }

    public void ReviewShown() { SendEvent("review_shown"); }
    public void ReviewAccept() { SendEvent("review_accept"); }
    public void ReviewDecline() { SendEvent("review_decline"); }
    public void AboutOpen() { SendEvent("about_open"); }
    public void LeaderboardOpen() { SendEvent("leaderboard_open"); }

    private void SendEvent(string eventName)
    {
        YG2.MetricaSend(eventName);
    }

    private void SendEvent(string eventName, Dictionary<string, string> eventData)
    {
        YG2.MetricaSend(eventName, eventData);
    }
}