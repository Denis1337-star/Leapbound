using Unity.Cinemachine;
using UnityEngine;

public sealed class CameraHitShake : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private float _force = 0.25f;

    public void Play()
    {
        if (_impulseSource == null)
            return;

        _impulseSource.GenerateImpulse(_force);
    }
}