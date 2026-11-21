public sealed class LevelRunContext
{
    public LevelRunMode Mode { get; private set; } = LevelRunMode.Normal;
    public float TargetTimeSeconds { get; private set; }
    public bool IsInputLocked { get; private set; }

    public void SetInputLocked(bool locked)
    {
        IsInputLocked = locked;
    }
    public void SetNormal()
    {
        Mode = LevelRunMode.Normal;
        TargetTimeSeconds = 0f;
    }

    public void SetTimeTrial(float targetTimeSeconds)
    {
        Mode = LevelRunMode.TimeTrial;
        TargetTimeSeconds = targetTimeSeconds;
    }
}