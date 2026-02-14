using UnityEngine;

public class PigEnemy : EnemyBase
{
    [Header("Patrol")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public int damage = 50;

    private Vector3 target;
    private SpriteRenderer sprite;
    private Collider2D col;

    protected override void Awake()
    {
        base.Awake();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        target = pointB.position;
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            target,speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            target = target == pointA.position ? pointB.position : pointA.position;
            sprite.flipX = !sprite.flipX;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        // проверяем, где контакт
        float playerBottom = collision.collider.bounds.min.y;
        float enemyTop = col.bounds.max.y;

        Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
        PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();


        // Если игрок прыгает сверху
        if (playerBottom > enemyTop - 0.05f)
        {
            // Враг умирает
            TakeDamage(maxHealth, Vector2.up);
        }
        else
        {
            // Игрок сталкивается сбоку или снизу -получает урон
            if (playerHealth != null)
                playerHealth.TakeDamage(damage, Vector2.zero);
        }
    }
}
