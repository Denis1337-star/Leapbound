using UnityEngine;
using Zenject;

public class EnemyAudio : ValidatedMonoBehaviour
{
    [SerializeField] private AudioClip _hurtClip;
    [SerializeField] private AudioClip _deathClip;

    private IAudioService _audioService;
    private EnemyBase _enemy;

    [Inject]
    public void Construct(IAudioService audioService)
    {
        _audioService = audioService;
    }
    protected override void Awake()
    {
        base.Awake();

        _enemy = GetComponent<EnemyBase>();
    }
    protected override bool ValidateInternal()
    {
        bool valid = true;
        valid &= ValidationUtility.IsAssigned(this, _enemy,nameof(_enemy));
        //valid &= ValidationUtility.IsAssigned(this, _hurtClip, nameof(_hurtClip));
        //valid &= ValidationUtility.IsAssigned(this, _deathClip, nameof(_deathClip));
        return valid;
    }

    private void OnEnable()
    {
        _enemy.OnDamaged += OnDamaged;
        _enemy.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        _enemy.OnDamaged -= OnDamaged;
        _enemy.OnDeath -= OnDeath;
    }

    private void OnDamaged(Vector2 _)
    {
        Play(_hurtClip);
    }

    private void OnDeath()
    {
        Play(_deathClip);
    }

    private void Play(AudioClip clip)
    {
        if (clip)
            _audioService.PlaySFX(clip);
    }
}
