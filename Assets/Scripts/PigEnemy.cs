using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigEnemy : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform pointA;  //точка для патруля
    public Transform pointB;
    public float speed = 2f;  //скорость движения

    [Header("Visual Settings")]
    [SerializeField] private float hitFlashTime = 0.15f; //длительность красного мерцния
    [SerializeField] private float deathFadeSpeed = 2f; // скорость исчезновения

    [Header("Audio")]
    public AudioClip[] footstepClips;  // звуки шагов
    public AudioClip deathClip;        // звук смерти
    private AudioSource audioSource;


    private Vector3 target;  //текущая цель А/В
    private SpriteRenderer sprite;
    private Rigidbody2D rb;


    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        target = pointB.position;  //движется вточку В
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        transform.position = Vector3.MoveTowards(
            transform.position, target, speed * Time.deltaTime  //для плавного движения 
        );

        if (Vector3.Distance(transform.position, target) < 0.05f)  //если до точки малое растсояние 
        {
            bool nearA = Vector3.Distance(target, pointA.position) < 0.1f;  //если близко к А-true
            target = nearA ? pointB.position : pointA.position;  //меняет точку target


            sprite.flipX = !sprite.flipX;  //зеркалит спрайт

            PlayFootstep();   // звук смены шага / поворота
        }
    }
    private void PlayFootstep()
    {
        if (footstepClips.Length == 0 || audioSource == null) return;  //проверяет на наличие звуков 

        int index = Random.Range(0, footstepClips.Length);  //выбирает рандомный звук из длинны трека
        audioSource.PlayOneShot(footstepClips[index]);  //воспроизводит звук по индексу
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;  //Если не с игроком выход

        PlayerController player = collision.collider.GetComponent<PlayerController>();  //пытается получить скрипт
        if (!player) return;

        Vector2 hitPoint = collision.GetContact(0).point;  //точка контакта 

        float playerBottom = player.transform.position.y - 0.4f;  //нижняя граница игрока
        float enemyTop = transform.position.y + 0.3f;  //верхняя граница 

        // игрок сверху - враг умирает
        if (playerBottom > enemyTop)
        {

            StartCoroutine(DieEffect());  //эффект смерти


            Rigidbody2D prb = player.GetComponent<Rigidbody2D>();
            prb.linearVelocity = new Vector2(prb.linearVelocity.x, 8f);  //подбрасывает игрока вверх

            PlayerScore.AddPoints(10); //добавляет очки 
        }
        else
        {
            // враг убивает игрока
            player.Die();
        }
    }



    private IEnumerator DieEffect()
    {
        //отключение колайдера
        GetComponent<Collider2D>().enabled = false;
        rb.linearVelocity = Vector2.zero;  //скорость 0

           // звук смерти
        if (audioSource && deathClip)
            audioSource.PlayOneShot(deathClip);

        //мерцание
        sprite.color = Color.red;
        yield return new WaitForSeconds(hitFlashTime);

        //возращаем цвет
        sprite.color = Color.white;
        
       // плавное исчезновение
       float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime *deathFadeSpeed;
            sprite.color = new Color (1,1,1,alpha);
            yield return null;

        }
        Destroy(gameObject);
    }

}
