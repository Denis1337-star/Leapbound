using System.Collections;
using UnityEngine;

public class PlayerDeathEffect : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
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
        _rb.linearVelocity = Vector2.zero;  //останавливаем
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        PlayerScore.Lose();                // помечаем проигрыш
        _sprite.color = Color.red;
        yield return new WaitForSeconds(1.2f);

        GameManager.Instance?.RestartLevel();  // перезапуск сцены
    }
}
