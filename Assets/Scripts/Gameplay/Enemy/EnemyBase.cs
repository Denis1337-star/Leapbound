using System;
using UnityEngine;
using Zenject;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _hostCollider;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _deathClip;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private int _contactDamage = 50;
    private EnemyDeath _enemyDeath;
    private EnemyAudio _enemyAudio;
    private IAudioService _audioService;
    private IScoreService _scoreService;
    private Vector2 _lastHitDirection = Vector2.up;
    public bool IsDead { get; private set; }
    public event Action OnDeath;
    protected SpriteRenderer Sprite => _spriteRenderer;
    protected Collider2D BodyCollider => _hostCollider;
    protected Animator Animator => _animator;
    protected Rigidbody2D Rigidbody => _rigidbody;

    [Inject]
    public void Construct(IAudioService audioService, IScoreService scoreService)
    {
        _audioService = audioService;
        _scoreService = scoreService;
    }
    protected virtual void Awake()
    {
        _enemyDeath = new EnemyDeath(this, _hostCollider, _animator, _rigidbody);
        _enemyAudio = new EnemyAudio(_audioService,  _deathClip);
        OnDeath += _enemyAudio.PlayDeath;
    }
    private void OnDestroy()
    {
        OnDeath -= _enemyAudio.PlayDeath;
    }
    public void Kill(Vector2 hitDirection)
    {
        if (IsDead)
            return;

        _lastHitDirection = hitDirection;
        Die();
    }
    protected virtual void Die()
    {
        if (IsDead)
            return;

        IsDead = true;
        _scoreService.Add(10);
        OnDeath?.Invoke();
        _enemyDeath.Begin(_lastHitDirection);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsDead)
            return;

        if (!collision.collider.TryGetComponent(out Player player))
            return;

        float playerBottom = player.BodyCollider.bounds.min.y;
        float enemyTop = BodyCollider.bounds.max.y;


        if (playerBottom > enemyTop - 0.05f)
        {
            float side = MathF.Sign(transform.position.x - player.transform.position.x);
            if (MathF.Abs(side) < 0.1f)
                side = 1f;

            Kill( new Vector2(side, 1f));

            Vector2 veloccity = player.Rigidbody.linearVelocity;
            veloccity.y = player.PlayerConfig.StopmBounceForce;
            player.Rigidbody.linearVelocity = veloccity;
            player.PlayLandDust();
            return;
        }

        Vector2 hitDirection = (player.transform.position - transform.position).normalized;
        player.PlayerHealth.TakeDamage(_contactDamage, hitDirection);
    }
}
