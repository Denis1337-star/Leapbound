using UnityEngine;

public sealed class Player : MonoBehaviour
{
    [Header("Core Components")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] CapsuleCollider2D _bodyCollider;

    [Header("Gameplay Component")]
    [SerializeField] private PlayerMotor _playerMotor;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerAnimation _playerAnimation;

    [Header("Collider Setting")]
    [SerializeField] private Vector2 _standardSize;
    [SerializeField] private Vector2 _standardOffset;
    [SerializeField] private Vector2 _crouchSize;
    [SerializeField] private Vector2 _crouchOffset;

    public Rigidbody2D Rigidbody => _rigidbody;
    public Animator Animator => _animator;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public CapsuleCollider2D BodyCollider => _bodyCollider;
    public PlayerMotor PlayerMotor => _playerMotor;
    public PlayerHealth PlayerHealth => _playerHealth;
    public PlayerAnimation PlayerAnimation => _playerAnimation;

    private Vector2 _targetSize;
    private Vector2 _targetOffset;
    private void Awake()
    {
        SetCrouchCollider(false);   
    }
    public void SetCrouchCollider(bool isCrouching)
    {
        if (isCrouching)
        {
            _targetSize = _crouchSize;
            _targetOffset = _crouchOffset;
        }
        else
        {
            _targetOffset = _standardOffset;
            _targetSize = _standardSize;
        }

        _bodyCollider.size = _targetSize;
        _bodyCollider.offset = _targetOffset;
    }
}
