using System.Collections;
using UnityEngine;

public class PlayerDeathEffect : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Collider2D col;
    private Rigidbody2D rb;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        GetComponent<PlayerHealth>().OnDeath += Play;
    }

    private void OnDisable()
    {
        GetComponent<PlayerHealth>().OnDeath -= Play;
    }
    public void Play()
    {
        rb.linearVelocity = Vector2.zero;  //останавливаем
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {

        // Конец игры
        PlayerScore.Lose();                // помечаем проигрыш
        sprite.color = Color.red;
        yield return new WaitForSeconds(1.2f);

        GameManager.Instance?.RestartLevel();  // перезапуск сцены
    }
}
