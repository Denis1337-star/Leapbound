public sealed class LevelRunContext
{
    public LevelRunMode LevelMode { get; private set; } = LevelRunMode.Normal;
    public float TargetTimeSeconds { get; private set; }
    public bool IsInputLocked { get; private set; }
    public bool OpenLevelsOnMainMenu { get; private set; }
    public HeroId SelectedHero { get; private set; } = HeroId.Knight;

    public void SetInputLocked(bool locked)
    {
        IsInputLocked = locked;
    }
    public void SetNormal()
    {
        LevelMode = LevelRunMode.Normal;
        TargetTimeSeconds = 0f;
    }

    public void SetTimeTrial(float targetTimeSeconds)
    {
        LevelMode = LevelRunMode.TimeTrial;
        TargetTimeSeconds = targetTimeSeconds;
    }
    public void RequestOpenLevelsOnMainMenu()
    {
        OpenLevelsOnMainMenu = true;
    }
    public bool ConsumeOpenLevelsOnMainMenu()
    {
        bool value = OpenLevelsOnMainMenu;
        OpenLevelsOnMainMenu = false;
        return value;
    }
    public void SetHero(HeroId hero)
    {
        SelectedHero = hero;
    }
}