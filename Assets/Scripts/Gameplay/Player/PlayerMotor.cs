using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class PlayerMotor : ValidatedMonoBehaviour
{
    public float Move { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool RunHeld { get; private set; }
    public bool CrouchHeld { get; private set; }
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

    [Header("Collider")]
    [SerializeField] private float _heightMultiplier = 0.7f;

    private Rigidbody2D _rb;
    private CapsuleCollider2D _collider;
    private PlayerPlatformHandler _platformHandler;
    private Vector2 _originalSize;
    private Vector2 _originalOffset;
    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _platformHandler = GetComponent<PlayerPlatformHandler>();
        _originalSize = _collider.size;
        _originalOffset = _collider.offset;
    }
    protected override bool ValidateInternal()
    {
        bool valid = true;
        valid &= ValidationUtility.IsAssigned(this, _groundCheck, nameof(_groundCheck));
        valid &= ValidationUtility.IsAssigned(this, _ceilingCheck, nameof(_ceilingCheck));
        return valid;
    }
    private void Update()
    {
        ReadInput();
        CheckGrounded();
        UpdateCrouch();
        HandleJump();
    }
    private void FixedUpdate()
    {
        ApplyMovement();
    }
    private void ReadInput()
    {
        Move = Input.GetAxisRaw("Horizontal");
        RunHeld = Input.GetKey(KeyCode.LeftShift);
        CrouchHeld = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        if (Input.GetButtonDown("Jump"))
            JumpPressed = true;
    }
    public void ConsumeJump()
    {
        JumpPressed = false;
    }

    private void CheckGrounded()
    {
        IsGrounded = Physics2D.OverlapCircle(
            _groundCheck.position, _groundRadius, _groundLayer);
    }
    private void HandleJump()
    {
        if (!JumpPressed || !IsGrounded)
            return;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
        ConsumeJump();
    }
    private void UpdateCrouch()
    {
        bool ceilingBlocked = Physics2D.OverlapCircle(
            _ceilingCheck.position, _ceilingRadius, _groundLayer);
        IsCrouching = CrouchHeld || ceilingBlocked;
        UpdateCollider();
    }
    private void UpdateCollider()
    {
        if (IsCrouching)
        {
            _collider.size = new Vector2(
                _originalSize.x, _originalSize.y * _heightMultiplier);
            _collider.offset = new Vector2(
                _originalOffset.x,
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
        float speed = RunHeld ? _runSpeed : _walkSpeed;
        if (IsCrouching)
            speed *= _crawlSpeedMultiplier;
        float platformX = _platformHandler != null
            ? _platformHandler.PlatformVelocity.x
            : 0f;
        Vector2 velocity = _rb.linearVelocity;
        velocity.x = Move * speed + platformX;
        _rb.linearVelocity = velocity;
    }
}
