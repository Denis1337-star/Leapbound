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
    private PlayerHealth _playerHealth;
    private PlayerAudio _playerAudio;
    private readonly ContactPoint2D[] _contactPoints = new ContactPoint2D[8];
    public Vector2 PlatformVelocity { get; private set; }
    public PlayerConfig PlayerConfig => _playerConfig;
    public Rigidbody2D Rigidbody => _rigidbody;
    public Animator Animator => _animator;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public Transform GroundCheckAnchor => _groundCheckAnchor;
    public PlayerHealth PlayerHealth => _playerHealth;
    public Collider2D BodyCollider => _bodyCollider;

    [Inject]
    public void Construct(PlayerHealth playerHealth, PlayerAudio playerAudio)
    {
        _playerHealth = playerHealth;
        _playerAudio = playerAudio;
    }

    public void FreezeBody()
    {
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent(out Platform platform))
            return;

        int count = collision.GetContacts(_contactPoints);
        for(int i = 0; i < count; i++)
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
    public void PlayWalkStep(){ _playerAudio.PlayWalkStep(); }
    public void PlayRunStep() { _playerAudio.PlayRunStep(); }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_groundCheckAnchor.position, _playerConfig.GroundCheckRadius);
    }

}
