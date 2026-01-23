using UnityEngine;

public class BulletPlaint : MonoBehaviour
{
    public float speed = 8f;   //скорость пули
    public float lifeTime = 5f;  //время жизни пули
    public int damage = 50;  //сколько урона наносит

    private Vector2 direction;  //вектор направления пули 

    public void Setup(Vector2 dir)
    {
        direction = dir.normalized;  //нормализует длинну(=1)

        // Автоудаление через N секунд
        Destroy(gameObject, lifeTime);  //удалает обьект через lifeTime
    }

    private void Update()
    {
        // Движение пули
        transform.Translate(direction * speed * Time.deltaTime);  //в направление direction 
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Попадание в игрока
        if (col.CompareTag("Player"))
        {
            
            var health = col.GetComponent<PlayerHealthUI>(); //Пытается получить у обьекта компонент
            if (health != null)
                health.TakeDamage(damage); //если найден наносит урон

            Destroy(gameObject); //исчезает
            
        }

        // Попадание в землю
        if (col.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

}
