using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealthUI : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHP = 100;  //макс количества здоровья
    public int currentHP;  //текущее 

    [Header("UI Elements")]
    public Text healthText;  //отображает текущее ХП

    [Header("Damage Effect")]
    public float invincibleTime = 0.6f;     // время неуязвимости
    public float knockbackForce = 5f;       // сила отбрасывания
    public Color damageColor = new Color(1, 0.3f, 0.3f); // красный 
    private Color normalColor = Color.white;  //исходный цвет спрайта

    private bool isInvincible = false;  //флаг неуязвимости
    private SpriteRenderer sprite;  
    private Rigidbody2D rb;
    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();  //ссылка на ск
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        currentHP = maxHP;
        UpdateHealthUI();
    }

    private void Update()
    {  //Постоянно обновляет показательхп в UI
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (controller.isDead || isInvincible)  //если игрок мерт или неуязвим выход
            return;

        currentHP -= damage;  //вычитает из текущего хп урон который приходит из вне

        if (currentHP <= 0)   //проверка хп 
        {
            currentHP = 0;
            controller.Die();
        }

        // Запускаем эффект урона
        StartCoroutine(DamageEffect());

        UpdateHealthUI();
    }

    private IEnumerator DamageEffect()
    {
        GetComponent<PlayerAudio>().PlayHurt();  //вкл аудиоэффект
        isInvincible = true;  //неуязвим

        // 1. Отбрасывание
        float dir = transform.localScale.x > 0 ? -1 : 1; // задает вектор по маштабу спрайта (назад)
        rb.linearVelocity = new Vector2(dir * knockbackForce, knockbackForce);  //дает имппульс 

        // 2. Покраснение + мигание
        sprite.color = damageColor; 

        float blinkTimer = invincibleTime;  //таймер мигания
        while (blinkTimer > 0)
        {   
            //включает\выкл спрайт 
            sprite.enabled = false;
            yield return new WaitForSeconds(0.08f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.08f);

            blinkTimer -= 0.16f;
        }

        sprite.color = normalColor;  //нормального цвета
        isInvincible = false;  //уязвим
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)   //если текс назначет обновляет 
            healthText.text = $"HP {currentHP}/{maxHP}";
    }
    public void Heal(int amount)
    {
        currentHP += amount;  //увеличитвает хп на n число из вне
        if (currentHP > maxHP) currentHP = maxHP;  //нельзя чтобы текущее хп было больше макс
    }
}
