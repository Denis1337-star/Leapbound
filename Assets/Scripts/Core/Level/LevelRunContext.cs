public sealed class LevelRunContext
{
    public LevelRunMode LevelMode { get; private set; } = LevelRunMode.Normal;
    public float TargetTimeSeconds { get; private set; }
    public bool IsInputLocked { get; private set; }
    public bool OpenLevelsOnMainMenu { get; private set; }

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
}