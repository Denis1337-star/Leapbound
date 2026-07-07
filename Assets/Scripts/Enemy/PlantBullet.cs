using UnityEngine;

public class PlantBullet : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;   //скорость пули
    [SerializeField] private float _lifeTime = 5f;  //время жизни пули
    [SerializeField] private int _damage = 50;  //сколько урона наносит

    private Vector2 direction;  //вектор направления пули 

    public void Setup(Vector2 dir)
    {
        direction = dir.normalized;  //нормализует длинну(=1)

        // Автоудаление через N секунд
        Destroy(gameObject, _lifeTime);  //удалает обьект через lifeTime
    }

    private void Update()
    {
        // Движение пули
        transform.Translate(direction * _speed * Time.deltaTime);  //в направление direction 
    }

    private void OnTriggerEnter2D(Collider2D colider)
    {
        if (!colider.CompareTag("Player"))
        {
            // Попадание в землю
            if (colider.CompareTag("Ground"))
            {
                Destroy(gameObject);
            }
            return;
        }

        var health = colider.GetComponent<PlayerHealth>();
        if (health != null)
        {
            //Рассчитываем направление удара
            Vector2 hitDir = (colider.transform.position - transform.position).normalized;

            //наносим урон цели
            health.TakeDamage(_damage, hitDir);

            Destroy(gameObject);
            return;
        }
    }
}
