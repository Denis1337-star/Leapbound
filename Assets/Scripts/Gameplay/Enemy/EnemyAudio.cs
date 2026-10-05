using UnityEngine;
using Zenject;

public class EnemyAudio : MonoBehaviour
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
    private void Awake()
    {
        _enemy = GetComponent<EnemyBase>();
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
