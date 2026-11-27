using UnityEngine;

[CreateAssetMenu(fileName = "CameraFeelConfig", menuName = "Leapbound/Camera Feel Config")]
public sealed class CameraFeelConfig : ScriptableObject
{
    [Header("Screen X (0 = center, negative = character left)")]
    public float IdleScreenX ;
    public float WalkScreenX ;
    public float RunScreenX ;
    public float ScreenLerpSpeed;

    [Header("Facing")]
    public float FacingDeadZone ;
}