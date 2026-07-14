using System.Collections;
using UnityEngine;


//Визуальны эффект получения урона игроком
[RequireComponent (typeof(PlayerHealth))]
public class PlayerDamageFeedback : MonoBehaviour
{
    [SerializeField] private float _knockbackForce = 6f; //Сила отбрасывания
    [SerializeField] private float _invincibleTime = 1f; //Время после урона в неузявимости
    [SerializeField] private Color _damageColor = Color.red;  //Цвет спрайта при получение урона
    [SerializeField] private Color _normalColor = Color.white;  //Исходный цвет

    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;
    private PlayerHealth _health;
    private PlayerAudio _audioSource;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _health = GetComponent<PlayerHealth>();
        _audioSource = GetComponent<PlayerAudio>();
    }

    //При активации обьекта
    private void OnEnable()
    {
        //При срабатывании OnDamaged 
        _health.OnDamaged += StartDamageEffect;
    }
    private void OnDisable()
    {
       _health.OnDamaged -= StartDamageEffect;
    }
    private void StartDamageEffect(Vector2 hitDir)
    {
        StopAllCoroutines(); //чтобы избежать наложение эффекта
        StartCoroutine(DamageEffect(hitDir));
        
    }
    private IEnumerator DamageEffect(Vector2 hitDir)
    {

        _audioSource?.PlayHurt(); //Звук урона
        _health.SetInvincible(true); //Неязвим на время

        //Обнуляем текущию скоростьи применяем силу отбрасывания
        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(hitDir.normalized * _knockbackForce,ForceMode2D.Impulse);

        _sprite.color = _damageColor;  //меняем цвет

        //эффект мигания
        float timer = _invincibleTime;
        while (timer > 0)
        {
            _sprite.enabled = false;
            yield return new WaitForSeconds(0.08f); 
            _sprite.enabled = true;
            yield return new WaitForSeconds(0.08f); 
            timer -= 0.16f; 
        }
        //возрощаем исходные данные
        _sprite.color = _normalColor;
        _health.SetInvincible(false);
    }    
}
