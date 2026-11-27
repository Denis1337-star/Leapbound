using UnityEngine;

[CreateAssetMenu(fileName = "TouchInputConfig", menuName = "Leapbound/Touch Input Config")]
public sealed class TouchInputConfig : ScriptableObject
{
    [Range(0f, 0.4f)] public float DeadZone;
    [Range(0.3f, 0.95f)] public float RunThreshold ;
}