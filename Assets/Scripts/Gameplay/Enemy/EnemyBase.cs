using System;
using UnityEngine;
using Zenject;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 50;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _hostCollider;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _hurtClip;
    [SerializeField] private AudioClip _deathClip;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private int _contactDamage = 50;
    private EnemyDeath _enemyDeath;
    private EnemyAudio _enemyAudio;
    private IAudioService _audioService;
    private Vector2 _lastHitDirection = Vector2.up;
    public int CurrentHP { get; private set; }
    public bool IsDead { get; private set; }
    public event Action<Vector2> OnDamaged;
    public event Action OnDeath;
    public int MaxHealth => _maxHealth;
    protected SpriteRenderer Sprite => _spriteRenderer;
    protected Collider2D BodyCollider => _hostCollider;
    protected Animator Animator => _animator;
    protected Rigidbody2D Rigidbody => _rigidbody;

    [Inject]
    public void Construct(IAudioService audioService)
    {
        _audioService = audioService;
    }
    protected virtual void Awake()
    {
        CurrentHP = _maxHealth;
        _enemyDeath = new EnemyDeath(this, _spriteRenderer, _hostCollider, _animator, _rigidbody );
        _enemyAudio = new EnemyAudio(_audioService, _hurtClip, _deathClip);
        OnDamaged += _enemyAudio.PlayHurt;
        OnDeath += _enemyAudio.PlayDeath;
    }
    private void OnDestroy()
    {
        OnDamaged -= _enemyAudio.PlayHurt;
        OnDeath -= _enemyAudio.PlayDeath;
    }

    public virtual void TakeDamage(int amount, Vector2 hitDirection)
    {
        if (CurrentHP <= 0)
            return;

        CurrentHP -= amount;
        CurrentHP = Mathf.Max(CurrentHP, 0);
        _lastHitDirection = hitDirection;
        OnDamaged?.Invoke(hitDirection);

        if (CurrentHP <= 0)
            Die();
    }

    protected virtual void Die()
    {
        if (IsDead)
            return;
        IsDead = true;
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

            TakeDamage(MaxHealth, new Vector2(side, 1f));
            return;
        }

        Vector2 hitDirection = (player.transform.position - transform.position).normalized;
        player.PlayerHealth.TakeDamage(_contactDamage, hitDirection);
    }
}
