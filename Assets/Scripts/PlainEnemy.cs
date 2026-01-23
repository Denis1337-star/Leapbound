using UnityEngine;
using UnityEngine.UIElements;

public class PlainEnemy : MonoBehaviour
{
    [Header("Shooting")]
    public Transform firePoint;       // Точка, откуда летят пули
    public GameObject bulletPrefab;   // Префаб пули
    public float fireInterval = 1.5f; // Задержка между выстрелами
    public Vector2 shootDirection = Vector2.left;  // можно менять в инспекторе направление

    [Header("Audio")]
    public AudioClip shootSound;         // звук выстрела
    private AudioSource audioSource;     // аудиоисточник врага

    private float fireTimer;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // ищем или создаём аудиоисточник на враге
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;   // 3D-звук (как в играх)
    }

    private void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireInterval)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    private void Shoot()
    {
        // Включить анимацию атаки
        animator.SetTrigger("Shoot");

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);  //создает  пулю в точке 

        // Получаем скрипт пули
        BulletPlaint b = bullet.GetComponent<BulletPlaint>();

        // Передаём направление
        b.Setup(shootDirection);

        // проигрываем звук
        if (shootSound != null)
            audioSource.PlayOneShot(shootSound);
    }
}
