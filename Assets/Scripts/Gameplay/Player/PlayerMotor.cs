using UnityEngine;
using Zenject;

public class PlayerMotor : ValidatedMonoBehaviour
{
    public float Move => _inputService.Move;
    public bool RunHeld => _inputService.RunHeld;
    public bool CrouchHeld => _inputService.CrouchHeld;
    public bool JumpPressed => _inputService.JumpPressed;
    public bool IsCrouching { get; private set; }
    public bool IsGrounded { get; private set; }

    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 3f;
    [SerializeField] private float _runSpeed = 6f;
    [SerializeField] private float _crawlSpeedMultiplier = 0.5f;
    [SerializeField] private float _jumpForce = 7f;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Ceiling Check")]
    [SerializeField] private Transform _ceilingCheck;
    [SerializeField] private float _ceilingRadius = 0.2f;
    [SerializeField] private float _heightMultiplier = 0.7f;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private PlayerPlatformHandler _platformHandler;
    private IInputService _inputService;
    private Vector2 _originalSize;
    private Vector2 _originalOffset;

    [Inject]
    public void Construct(IInputService inputService)
    {
        _inputService = inputService;
    }
    protected override void Awake()
    {
        base.Awake();

        _originalSize = _collider.size;
        _originalOffset = _collider.offset;
    }
    protected override bool ValidateInternal()
    {
        bool valid = true;
        valid &= ValidationUtility.IsAssigned(this, _groundCheck, nameof(_groundCheck));
        valid &= ValidationUtility.IsAssigned(this, _ceilingCheck, nameof(_ceilingCheck));
        valid &= ValidationUtility.IsAssigned(this, _rigidbody, nameof(_rigidbody));
        valid &= ValidationUtility.IsAssigned(this, _collider, nameof(_collider));
        valid &= ValidationUtility.IsAssigned(this, _platformHandler, nameof(_platformHandler));
        return valid;
    }
    private void Update()
    {
        CheckGrounded();
        UpdateCrouch();
        HandleJump();
    }
    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void CheckGrounded()
    {
        IsGrounded = Physics2D.OverlapCircle(
            _groundCheck.position, _groundRadius, _groundLayer);
    }
    private void HandleJump()
    {
        if (!_inputService.JumpPressed || !IsGrounded)
            return;

        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _jumpForce);
        _inputService.ConsumeJump();
    }
    private void UpdateCrouch()
    {
        bool ceilingBlocked = Physics2D.OverlapCircle(
            _ceilingCheck.position, _ceilingRadius, _groundLayer);

        IsCrouching = _inputService.CrouchHeld || ceilingBlocked;
        UpdateCollider();
    }
    private void UpdateCollider()
    {
        if (IsCrouching)
        {
            _collider.size = new Vector2(
                _originalSize.x, _originalSize.y * _heightMultiplier);

            _collider.offset = new Vector2(_originalOffset.x,
                _originalOffset.y - (_originalSize.y - _collider.size.y) / 2f);
        }
        else
        {
            _collider.size = _originalSize;
            _collider.offset = _originalOffset;
        }
    }
    private void ApplyMovement()
    {
        float speed;
        if (_inputService.RunHeld)
            speed = _runSpeed;
        else
            speed = _walkSpeed;

        if (IsCrouching)
            speed *= _crawlSpeedMultiplier;

        float platformX;
        if (_platformHandler != null)
            platformX = _platformHandler.PlatformVelocity.x;
        else
            platformX = 0f;
        
        Vector2 velocity = _rigidbody.linearVelocity;
        velocity.x = _inputService.Move * speed + platformX;
        _rigidbody.linearVelocity = velocity;
    }
}
