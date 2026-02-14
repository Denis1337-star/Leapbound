using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioClip hurtClip;
    public AudioClip deathClip;

    private AudioSource source;
    private EnemyBase enemy;

    private void Start()
    {
        
    }
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        enemy = GetComponent<EnemyBase>();
    }

    private void OnEnable()
    {
        enemy.OnDamaged += OnDamaged;
        enemy.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        enemy.OnDamaged -= OnDamaged;
        enemy.OnDeath -= OnDeath;
    }

    private void OnDamaged(Vector2 _)
    {
        Play(hurtClip);
    }

    private void OnDeath()
    {
        Play(deathClip);
    }

    private void Play(AudioClip clip)
    {
        if (clip)
            source.PlayOneShot(clip);
    }
}
