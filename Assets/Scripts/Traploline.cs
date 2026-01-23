using UnityEngine;

public class Traploline : MonoBehaviour
{
    [Header("Trampoline Settings")]
    public float launchForce = 14f;     // сила подкидывани€
    public float delayBeforeJump = 1f;  // задержка перед запуском

    private Animator animator;  //ссылка
    private bool playerOnPlatform = false;  //флаг есть\нет игрока на трамплине
    private float timer = 0f;  //таймер перед подбрасыванием

    private void Awake()
    {
        animator = GetComponent<Animator>(); //ищем компонет
    }

    private void Update()
    {
        if (playerOnPlatform)  //если игрок на месте
        {
            timer += Time.deltaTime;  //увеличиваем таймер

            // через секунду Ч запуск
            if (timer >= delayBeforeJump)
            {
                LaunchPlayer();
                timer = 0f;
                playerOnPlatform = false;
            }
        }
    }

    private void LaunchPlayer()
    {
        animator.SetTrigger("Jump"); // проиграть анимацию

        // найти игрока на платформе(в радиусе сверху )
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(1.2f, 0.5f), 0); 

        foreach (var col in cols) //провер€ет все колайдеры
        {
            if (col.CompareTag("Player"))   //если игрок
            {
                Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, launchForce);  //дает скорость равной force
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = true;
            timer = 0f;
            animator.SetTrigger("Idle"); // на вс€кий случай
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = false;
            timer = 0f;
        }
    }
}
