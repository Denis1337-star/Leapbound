using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("Trampoline Settings")]
    [SerializeField] private float _launchForce = 14f;     // сила подкидывания
    [SerializeField] private float _delayBeforeJump = 1f;  // задержка перед запуском

    private Animator _animator;  //ссылка
    private bool _playerOnPlatform = false;  //флаг есть\нет игрока на трамплине
    private float _timer = 0f;  //таймер перед подбрасыванием

    private void Awake()
    {
        _animator = GetComponent<Animator>(); //ищем компонет
    }

    private void Update()
    {
        if (_playerOnPlatform)  //если игрок на месте
        {
            _timer += Time.deltaTime;  //увеличиваем таймер

            // через секунду — запуск
            if (_timer >= _delayBeforeJump)
            {
                LaunchPlayer();
                _timer = 0f;
                _playerOnPlatform = false;
            }
        }
    }

    private void LaunchPlayer()
    {
        _animator.SetTrigger("Jump"); // проиграть анимацию

        // найти игрока на платформе(в радиусе сверху )
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(1.2f, 0.5f), 0); 

        foreach (var col in cols) //проверяет все колайдеры
        {
            if (col.CompareTag("Player"))   //если игрок
            {
                Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, _launchForce);  //дает скорость равной force
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _playerOnPlatform = true;
            _timer = 0f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _playerOnPlatform = false;
            _timer = 0f;
        }
    }
}
