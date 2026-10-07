using UnityEngine;
using Zenject;

public class PlayerMotor : MonoBehaviour
{
    public float HorizontalInput { get; private set; }
    public bool IsRunHeld { get; private set; }
    public bool IsCrouchHeld { get; private set; }
    public bool IsJumpPressed { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsCrouchingNow { get; private set; }
    public bool IsCeiling { get; private set; }

    [Header("Movement")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _crawlSpeedMultiplier;
    [SerializeField] private float _jumpForce;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheckAnchor;
    [SerializeField] private float _groundCheckRadius ;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private PlayerPlatformHandler _platformHandler;

    [Header("Ceiling Check")]
    [SerializeField] private Transform _ceilingCheckAnchor;
    [SerializeField] private float _ceilingCheckRadius ;

    private IInputService _inputService;
    private Player _player;

    [Inject]
    public void Construct(IInputService inputService, Player player)
    {
        _inputService = inputService;
        _player = player;
    }
    private void Update()
    {
        ReadInputState();
        CheckGrounded();
        UpdateCrouchState();
        TryHandleJump();
    }

    private void FixedUpdate()
    {
        ApplyHorizontalMovement();
    }
    private void ReadInputState()
    {
        HorizontalInput = _inputService.Move;
        IsRunHeld = _inputService.RunHeld;
        IsCrouchHeld = _inputService.CrouchHeld;
        IsJumpPressed = _inputService.JumpPressed;
    }
    private void CheckGrounded()
    {
        IsGrounded = Physics2D.OverlapCircle(_groundCheckAnchor.position, _groundCheckRadius, _groundLayer);
    }
    private void UpdateCrouchState()
    {
        IsCeiling = Physics2D.OverlapCircle(_ceilingCheckAnchor.position, _ceilingCheckRadius, _groundLayer);
        bool shouldCrouch = IsCrouchHeld || IsCeiling;
        if(shouldCrouch != IsCrouchingNow)
        {
            IsCrouchingNow = shouldCrouch;
            _player.SetCrouchCollider(IsCrouchingNow);
        }
    }
    private void TryHandleJump()
    {
        if (!IsJumpPressed || !IsGrounded)
            return;

        _player.Rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        _inputService.ConsumeJump();
    }
    private void ApplyHorizontalMovement()
    {
        float moveSpeed;
        if (IsRunHeld)
            moveSpeed = _runSpeed;
        else
            moveSpeed = _walkSpeed;

        if (IsCrouchingNow)
            moveSpeed *= _crawlSpeedMultiplier;

        float platformVelocityX = 0f;
        platformVelocityX = _platformHandler.PlatformVelocity.x;

        Vector2 finalVelocity = _player.Rigidbody.linearVelocity;
        finalVelocity.x = (HorizontalInput * moveSpeed) + platformVelocityX;
        _player.Rigidbody.linearVelocity = finalVelocity;
    }
}
