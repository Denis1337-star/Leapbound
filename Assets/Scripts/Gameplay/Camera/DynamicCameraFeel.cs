using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public sealed class DynamicCameraFeel : ILateTickable
{
    private readonly PlayerMotor _playerMotor;
    private readonly CinemachinePositionComposer _cameraComposer;
    private readonly CameraFeelConfig _cameraConfig;
    private float _currentScreenX;
    private float _facingSign = 1f;

    public DynamicCameraFeel(PlayerMotor playerMotor,CinemachinePositionComposer cameraComposer,
        CameraFeelConfig cameraConfig)
    {
        _playerMotor = playerMotor;
        _cameraComposer = cameraComposer;
        _cameraConfig = cameraConfig;
        _currentScreenX = _cameraConfig.IdleScreenX;
    }

    public void LateTick()
    {
        UpdateScreenBias();
    }

    private void UpdateScreenBias()
    {
        float moveDirection = _playerMotor.MoveDirection;
        float absMove = Mathf.Abs(moveDirection);

        if (absMove > _cameraConfig.FacingDeadZone)
        {
            if (moveDirection > 0f)
                _facingSign = 1f;
            else
                _facingSign = -1f;
        }

        float targetAbs = _cameraConfig.IdleScreenX;
        if (absMove > _cameraConfig.FacingDeadZone)
        {
            if (_playerMotor.IsRunButtonHeld)
                targetAbs = _cameraConfig.RunScreenX;
            else
                targetAbs = _cameraConfig.WalkScreenX;
        }

        float targetX = targetAbs * _facingSign;
        _currentScreenX = Mathf.Lerp(_currentScreenX,targetX,
            1f - Mathf.Exp(-_cameraConfig.ScreenLerpSpeed * Time.deltaTime));

        ScreenComposerSettings composition = _cameraComposer.Composition;
        Vector2 screenPosition = composition.ScreenPosition;
        screenPosition.x = _currentScreenX;
        composition.ScreenPosition = screenPosition;
        _cameraComposer.Composition = composition;
    }
}