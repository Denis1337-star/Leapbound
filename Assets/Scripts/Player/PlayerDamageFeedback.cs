using System.Collections;
using UnityEngine;


//Визуальны эффект получения урона игроком
[RequireComponent (typeof(PlayerHealth))]
public class PlayerDamageFeedback : MonoBehaviour
{
    public float knockbackForce = 6f; //Сила отбрасывания
    public float invincibleTime = 1f; //Время после урона в неузявимости
    public Color damageColor = Color.red;  //Цвет спрайта при получение урона
    public Color normalColor = Color.white;  //Исходный цвет

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private PlayerHealth health;
    private PlayerAudio audioSource;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        health = GetComponent<PlayerHealth>();
        audioSource = GetComponent<PlayerAudio>();
    }

    //При активации обьекта
    private void OnEnable()
    {
        //При срабатывании OnDamaged 
        health.OnDamaged += StartDamageEffect;
    }
    private void OnDisable()
    {
       health.OnDamaged -= StartDamageEffect;
    }
    private void StartDamageEffect(Vector2 hitDir)
    {
        StopAllCoroutines(); //чтобы избежать наложение эффекта
        StartCoroutine(DamageEffect(hitDir));
        
    }
    private IEnumerator DamageEffect(Vector2 hitDir)
    {

        audioSource?.PlayHurt(); //Звук урона
        health.SetInvincible(true); //Неязвим на время

        //Обнуляем текущию скоростьи применяем силу отбрасывания
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(hitDir.normalized * knockbackForce,ForceMode2D.Impulse);

        sprite.color = damageColor;  //меняем цвет

        //эффект мигания
        float timer = invincibleTime;
        while (timer > 0)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.08f); //такое время скрыт спрайт
            sprite.enabled = true;
            yield return new WaitForSeconds(0.08f); //такое время виден спрайт
            timer -= 0.16f; //Уменьшаем таймиер
        }
        //возрощаем исходные данные
        sprite.color = normalColor;
        health.SetInvincible(false);
    }    
}
