using UnityEngine;

public sealed class FallingPlatform : MonoBehaviour
{
    private static readonly int IsOffHash = Animator.StringToHash("IsOff");
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private float _delay;
    [SerializeField] private float _hideDelay;
    [SerializeField] private float _gravityScale;
    [SerializeField] private float _respawnDelay;

    private readonly ContactPoint2D[] _contactPoin= new ContactPoint2D[4];
    private Vector2 _startPosition;
    private float _timer = -1f;
    private float _respawnTimer = -1f;
    private float _hideTimer = -1f;
    private bool _falling;
    private void Awake()
    {
        _startPosition = _rigidbody.position;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _rigidbody.gravityScale = 0f;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryStart(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        TryStart(collision);
    }
    private void TryStart(Collision2D collision)
    {
        if (_falling || _timer >= 0f)
            return;
        if (!collision.collider.TryGetComponent(out Player _))
            return;
        if (!IsStandingOnTop(collision))
            return;
        _timer = _delay;
        _animator.SetBool(IsOffHash, true);
        _hideTimer = _hideDelay;
    }
    private void Update()
    {
        if (_timer >= 0f)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer = -1f;
                Fall();
            }
        }

        if(_hideTimer >=0f)
        {
            _hideTimer -= Time.deltaTime;
            if(_hideTimer <=0f)
            {
                _hideTimer = -1f;
                Hide();
            }
        }

        if (_respawnTimer < 0f)
            return;
        _respawnTimer -= Time.deltaTime;
        if (_respawnTimer > 0f)
            return;
        Respawn();
    }
    private void Fall()
    {
        if (!_collider.enabled)
            return;

        _falling = true;
        _respawnTimer = _respawnDelay;
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody.gravityScale = _gravityScale;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
    }
    private void Respawn()
    {
        _falling = false;
        _timer = -1f;
        _respawnTimer = -1f;
        _hideTimer = -1f;
        _spriteRenderer.enabled = true;
        _collider.enabled = true;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _rigidbody.gravityScale = 0f;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rigidbody.position = _startPosition;
        Physics2D.SyncTransforms();
        _animator.SetBool(IsOffHash, false);
    }
    private bool IsStandingOnTop(Collision2D collision)
    {
        int count = collision.GetContacts(_contactPoin);
        for (int i = 0; i < count; i++)
        {
            if (_contactPoin[i].normal.y < -0.5f)
                return true;
        }
        return false;
    }
    private void Hide()
    {
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _rigidbody.gravityScale = 0f;
    }
}
