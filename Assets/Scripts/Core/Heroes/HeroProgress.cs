using YG;

public sealed class HeroProgress
{
    public bool IsUnlocked(HeroId heroId)
    {
        if (heroId == HeroId.Knight)
            return true;
        if (heroId == HeroId.Mage)
            return false;
        return YG2.saves.rogueUnlocked;
    }
    public bool HasTrial(HeroId heroId)
    {
        if (heroId != HeroId.Rogue)
            return false;
        return YG2.saves.rogueTrialReady;
    }
    public bool CanSelect(HeroId heroId)
    {
        if (heroId == HeroId.Mage)
            return false;
        if (IsUnlocked(heroId))
            return true;
        return HasTrial(heroId);
    }
    public void GrantRogueTrial()
    {
        YG2.saves.rogueTrialReady = true;
        YG2.SaveProgress();
    }
    public void UnlockRogue()
    {
        YG2.saves.rogueUnlocked = true;
        YG2.saves.rogueTrialReady = false;
        YG2.SaveProgress();
    }
    public void ConsumeRogueTrialIfNeeded(HeroId playedHero)
    {
        if (playedHero != HeroId.Rogue)
            return;
        if (YG2.saves.rogueUnlocked)
            return;
        if (!YG2.saves.rogueTrialReady)
            return;

        YG2.saves.rogueTrialReady = false;
        YG2.SaveProgress();
    }
    public void Equip(HeroId heroId, LevelRunContext levelRunContext)
    {
        if (!CanSelect(heroId))
            return;

        levelRunContext.SetHero(heroId);
        YG2.saves.selectedHeroId = (int)heroId;
        YG2.SaveProgress();
    }

    public void ApplySaved(LevelRunContext levelRunContext)
    {
        HeroId heroId = (HeroId)YG2.saves.selectedHeroId;
        if (!CanSelect(heroId))
            heroId = HeroId.Knight;

        levelRunContext.SetHero(heroId);
    }

    public void ResetToKnightIfNeeded(LevelRunContext levelRunContext)
    {
        ConsumeRogueTrialIfNeeded(levelRunContext.SelectedHero);
        if (!CanSelect(levelRunContext.SelectedHero))
            Equip(HeroId.Knight, levelRunContext);
    }
}