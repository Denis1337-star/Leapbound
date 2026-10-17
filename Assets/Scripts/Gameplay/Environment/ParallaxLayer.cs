using UnityEngine;

public sealed class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _parallaxFactor;
    [SerializeField] private Transform _leftSegment;
    [SerializeField] private Transform _rightSegment;
    [SerializeField] private float _spriteWidth = 40.94f;
    private float _startCameraX;
    private float _startLayerX;
    private void Awake()
    {
        _startCameraX = _cameraTransform.position.x;
        _startLayerX = transform.position.x;
    }
    private void LateUpdate()
    {
        float cameraX = _cameraTransform.position.x;
        float travelX = cameraX - _startCameraX;

        transform.position = new Vector3(_startLayerX + travelX * _parallaxFactor,
            transform.position.y, transform.position.z);

        Reposition(_leftSegment);
        Reposition(_rightSegment);
    }
    private void Reposition( Transform segment)
    {
        float offset = _cameraTransform.position.x - segment.position.x;

        if (offset > _spriteWidth)
            segment.position += Vector3.right * (_spriteWidth * 2f);
        else if (offset < -_spriteWidth)
            segment.position -= Vector3.right * (_spriteWidth * 2f);
    }
}
