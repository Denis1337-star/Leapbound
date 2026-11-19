using UnityEngine;

public sealed class PlatformCallZone : MonoBehaviour
{
    private enum BoundPoint { PointA, PointB }

    [SerializeField] private Platform _platform;
    [SerializeField] private BoundPoint _boundPoint;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Player player))
            return;

        if (_boundPoint == BoundPoint.PointA)
            _platform.CallToPointA();
        else
            _platform.CallToPointB();
    }
}