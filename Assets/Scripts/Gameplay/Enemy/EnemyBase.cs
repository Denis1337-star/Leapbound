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
    [SerializeField] private float _fadeSpeed = 1f;
    private EnemyDeath _enemyDeath;
    private EnemyAudio _enemyAudio;
    private IAudioService _audioService;
    public int CurrentHP { get; private set; }
    public bool IsDead { get; private set; }
    public event Action<Vector2> OnDamaged;
    public event Action OnDeath;
    public int MaxHealth => _maxHealth;
    protected SpriteRenderer Sprite => _spriteRenderer;
    protected Collider2D BodyCollider => _hostCollider;

    [Inject]
    public void Construct(IAudioService audioService)
    {
        _audioService = audioService;
    }
    protected virtual void Awake()
    {
        CurrentHP = _maxHealth;
        _enemyDeath = new EnemyDeath(this, _spriteRenderer, _hostCollider, _animator, _fadeSpeed);
        _enemyAudio = new EnemyAudio(_audioService, _hurtClip, _deathClip);
        OnDamaged += _enemyAudio.PlayHurt;
        OnDeath += _enemyAudio.PlayDeath;
    }
    private void OnDestroy()
    {
        OnDamaged -= _enemyAudio.PlayHurt;
        OnDeath -= _enemyAudio.PlayDeath;
    }

    public virtual void TakeDamage(int amount, Vector2 hitDir)
    {
        if (CurrentHP <= 0)
            return;

        CurrentHP -= amount;
        CurrentHP = Mathf.Max(CurrentHP, 0);

        OnDamaged?.Invoke(hitDir);

        if (CurrentHP <= 0)
            Die();
    }

    protected virtual void Die()
    {
        if (IsDead)
            return;
        IsDead = true;
        OnDeath?.Invoke();
        _enemyDeath.Begin();
    }
}
