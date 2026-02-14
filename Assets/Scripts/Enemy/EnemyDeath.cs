using System.Collections;
using UnityEngine;


public class EnemyDeath : MonoBehaviour
{
    public float fadeSpeed = 2f; //Скорость затухания

    private SpriteRenderer sprite;
    private Collider2D col;
    private Rigidbody2D rb;
    private EnemyBase enemy;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        enemy = GetComponent<EnemyBase>();
    }
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        enemy.OnDeath += Die;
    }

    private void OnDisable()
    {
        enemy.OnDeath -= Die;
    }
    private void Die()
    {
        col.enabled = false; //чтобы больше не взаимодействовал с игроком
        rb.linearVelocity = Vector2.zero; //Остснавливаем движение

        StartCoroutine(FadeOut());  //Для визуала
    }
    private IEnumerator FadeOut()
    {
        float alpha = 1f;  //исходные данные
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed; //уменьшаем прозрачность

            sprite.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        Destroy(gameObject);
    }
}
