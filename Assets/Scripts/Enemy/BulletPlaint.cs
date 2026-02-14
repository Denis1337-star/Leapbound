using UnityEngine;

public class BulletPlaint : MonoBehaviour
{
    public float speed = 8f;   //скорость пули
    public float lifeTime = 5f;  //врем€ жизни пули
    public int damage = 50;  //сколько урона наносит

    private Vector2 direction;  //вектор направлени€ пули 

    public void Setup(Vector2 dir)
    {
        direction = dir.normalized;  //нормализует длинну(=1)

        // јвтоудаление через N секунд
        Destroy(gameObject, lifeTime);  //удалает обьект через lifeTime
    }

    private void Update()
    {
        // ƒвижение пули
        transform.Translate(direction * speed * Time.deltaTime);  //в направление direction 
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var damagble = col.GetComponent<IDamageble>();
        if (damagble != null)
        {
            //–ассчитываем направление удара
            Vector2 hitDir = (col.transform.position - transform.position).normalized;

            //наносим урон цели
            damagble.TakeDamage(damage,hitDir);

            Destroy(gameObject);
            return;
        }
        // ѕопадание в землю
        if (col.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
