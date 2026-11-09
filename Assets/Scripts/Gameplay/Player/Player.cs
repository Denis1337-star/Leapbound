using UnityEngine;
using Zenject;

public sealed class Player : MonoBehaviour
{
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _bodyCollider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _groundCheckAnchor;
    [SerializeField] private PlayerSwordHit _playerSwordHit;
    [SerializeField] private HitStopPulse _hitStopPulse;
    [SerializeField] private CameraHitShake _cameraHitShake;
    private PlayerHealth _playerHealth;
    private PlayerAudio _playerAudio;
    private PlayerDust _playerDust;
    private bool _attackMoveLock;
    private readonly ContactPoint2D[] _contactPoints = new ContactPoint2D[8];
    public Vector2 PlatformVelocity { get; private set; }
    public Vector2 WindVelocity { get; private set; }
    public PlayerConfig PlayerConfig => _playerConfig;
    public Rigidbody2D Rigidbody => _rigidbody;
    public Animator Animator => _animator;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public Transform GroundCheckAnchor => _groundCheckAnchor;
    public PlayerHealth PlayerHealth => _playerHealth;
    public Collider2D BodyCollider => _bodyCollider;
    public PlayerSwordHit SwordHit => _playerSwordHit;
    public bool AttackMoveLock => _attackMoveLock;

    [Inject]
    public void Construct(PlayerHealth playerHealth, PlayerAudio playerAudio, PlayerDust playerDust)
    {
        _playerHealth = playerHealth;
        _playerAudio = playerAudio;
        _playerDust = playerDust;
    }

    public void FreezeBody()
    {
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    public void SnapOntoColliderTop(Collider2D surface)
    {
        Bounds surfaceBounds = surface.bounds;
        Bounds bodyBounds = _bodyCollider.bounds;
        float bottomOffset = _rigidbody.position.y - bodyBounds.min.y;
        float x = surfaceBounds.center.x;
        float y = surfaceBounds.max.y + bottomOffset;
        _rigidbody.position = new Vector2(x, y);
        Physics2D.SyncTransforms();
    }
    public void RestoreBody(Vector3 position)
    {
        PlatformVelocity = Vector2.zero;
        WindVelocity = Vector2.zero;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rigidbody.position = position;
        Physics2D.SyncTransforms();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent(out Platform platform))
            return;

        int count = collision.GetContacts(_contactPoints);
        for (int i = 0; i < count; i++)
        {
            if (_contactPoints[i].normal.y <= 0.5f)
                continue;

            PlatformVelocity = platform.Velocity;
            return;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Platform>(out _))
            PlatformVelocity = Vector2.zero;
    }
    public void PlayHitJuice()
    {
        _hitStopPulse.Play();
        _cameraHitShake.Play();
    }
    public void SetWindVelocity(Vector2 velocity) { WindVelocity = velocity; }
    public void ClearWindVelocity() { WindVelocity = Vector2.zero; }
    public void PlayWalkStep() { _playerAudio.PlayWalkStep(); }
    public void PlayRunStep() { _playerAudio.PlayRunStep(); }
    public void PlayAttackAudio() { _playerAudio.PlayAttack(); }
    public void PlayLandDust() { _playerDust.PlayLand(); }
    public void SetAttackMoveLock(bool locked) { _attackMoveLock = locked; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_groundCheckAnchor.position, _playerConfig.GroundCheckRadius);
    }

}
