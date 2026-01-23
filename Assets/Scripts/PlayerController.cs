using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3f;                //Скорость ходьбы
    public float runSpeed = 6f;                 //Скорость бега(при зажатом shift)
    public float jumpForce = 7f;                //Сила прыжка по Y
    public float crawlSpeedMultiplier = 0.5f;   //Множитель скорости при приседании(скорость/2)

    [Header("Ground Check")]
    public Transform groundCheck;               //Точка для проверки земли
    public float groundRadius = 0.2f;           //Радиус круга для проверки контакта с землей
    public LayerMask groundLayer;               //Маска слоев(каким слоем считать землей)

    [Header("Ceiling Check")]
    public Transform ceilingCheck;              //Точка для проверки припятсвия над головой
    public float ceilingRadius = 0.2f;          //радиус проверки потолка
    public float crawHeightMultiplier = 0.5f;   //На сколько уменьшается высота коллайдера при приседании

    private Rigidbody2D rb;                     //Физика 2Д
    private Animator animator;                 //Управление анимацией
    private SpriteRenderer sprite;             //управление спрайта
    private CapsuleCollider2D col;             //Управление колайдером

    private float moveInput;                  //Значение оси движения(-1 влево/0 стоим/ 1 вправо)
    private bool isGrounded;                  //на земле или нет
    public bool isRun;                        //состояния персонажа(бег)
    public bool isCrawl;                      //состояния персонажа(присел)
    public bool isDead;                       //состояния персонажа(умер)

    private float facing = 1f;                //Направление взгляда (1 вправо/-1 влево)

    private Vector2 originalSize;             //Исходные рамера коллайдера(чтобы востановить после приседания)
    private Vector2 originalOffset;           //Исходные данные местоположение коллайдера(чтобы востановить после приседания)

    private Platform currentPlatform;         //Ссылка на поатформу на которой стоит персонаж

    private void Awake()
    { 
        //Получаем ссылки на компоненты при запуске сцены(до Start)
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();

        //Сохраняет исходные размеры и местоположение коллайдера
        originalSize = col.size;
        originalOffset = col.offset;
    }

    private void Update()
    {//вызывается каждый кадр(зависит от FPS)

        if (isDead) return;           //Проверка персонаж умер или нет(если мертв ничего не делает)

        HandleInput();                //Обработка ввода с клавиатуры
        HandleAnimation();            //Управление анимации
        HandleFlip();                 //Разворот спрайта
        HandleCrawlCollider();        //Изменения коллайдера при приседании
    }

    private void FixedUpdate()
    {//Выполняется с фиксированной частатой(для физики)

        if (isDead) return;   //Проверка персонаж умер или нет(если мертв ничего не делает)

        GroundCheck();        //Проверят землю
        HandleMovement();     //Обрабатывает движения
    }

    // Собираем ввод с клавы
    private void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");   //получает горизонтальное движение(A/D) 
        isRun = Input.GetKey(KeyCode.LeftShift);      //Зажат ли LeftShift

        bool crawlPressed = Input.GetKey(KeyCode.LeftControl);  //Зажат ли LeftControl

        if (crawlPressed)   //если зажат Control
        {
            isCrawl = true;
        }
        else
        { //Если не нажат Control

            //Проверка препятствие над головой  — не можем выпрямиться
            isCrawl = Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, groundLayer);  //Если пересечение есть возращает true даже если не зажат control
        }
        //Проверка условий для прыжка
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrawl)  //Если нажат пробел на земле и не в приседе
            Jump(); //вызывается прыжок
    }

    // Движение (с учётом платформы)
    private void HandleMovement()
    {
        float speed = isRun ? runSpeed : walkSpeed; //Выбирает скорость движения от состояние персонажа 

        if (isCrawl)   //Снижение скорости при приседание
            speed *= crawlSpeedMultiplier;

        // горизонтальная скорость игрока
        float playerX = moveInput * speed;  //получает ось двжиения (влево\вправо\стоит)

        // Скорость платформы ТОЛЬКО по X
        float platformX = currentPlatform != null ? currentPlatform.PlatformVelocity.x : 0;

        // Итоговая скорость вместе с платформой
        rb.linearVelocity = new Vector2(playerX + platformX, rb.linearVelocity.y); 
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);  //Задает вертикальную скорость
        animator.SetTrigger("IsJump");  //активирует анимацию прыжка
        GetComponent<PlayerAudio>().PlayJump();  //проигрывает звук 
    }

    // Проверка земли(в воздухе или нет)
    private void GroundCheck()
    {
        bool wasGrounded = isGrounded;  //Запоминает был ли на земле в предыдущем кадре

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);  //Проверка косается ли земли(если да -  true)

        if (isGrounded && !wasGrounded) //момент приземления
            animator.ResetTrigger("IsJump"); //снимает триггер в аниматоре
    }

    // Управление анимацией
    private void HandleAnimation()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveInput));  //Передает значение без знака(0-idle)
        animator.SetBool("IsRun", isRun);  //От булевого значения(true - активирует)
        animator.SetBool("IsCrawl", isCrawl); //от булевого значения
    }

    // Разворот спрайта
    private void HandleFlip()
    {
        //Зависит от значения moveInput 
        if (moveInput > 0.1f) facing = -1;  
        else if (moveInput < -0.1f) facing = 1;

        sprite.flipX = facing < 0;  //разворачивает спрайт от направления
    }

    // Меняем высоту коллайдера при приседе
    private void HandleCrawlCollider()
    {
        if (isCrawl)
        {
            col.size = new Vector2(originalSize.x, originalSize.y * crawHeightMultiplier);  //Ширина остается высоты уменьшается
            //Вычисляет разницу высот до/после и смещение центра колайдера вниз на половину разницы(чтобы низ был в ногах)
            col.offset = new Vector2(originalOffset.x, originalOffset.y - (originalSize.y - col.size.y) / 2f); 
        }
        else
        {
            //возращает в исходное состояние
            col.size = originalSize;
            col.offset = originalOffset;
        }
    }

    //Смерть персонажа
    public void Die()
    {
        GetComponent<PlayerAudio>().PlayDeath(); //Проигрывает звук
        if (isDead) return; //Если мертв повторно не вызывается

        isDead = true; //Блокирует дальнейшее управление в update и тд

        rb.linearVelocity = Vector2.zero; //Обнуляет скорость
        animator.SetBool("IsDead", true); //активирует анимацию смерти

        StartCoroutine(DeathVisual());  //вызывает визуальный эффект

        Invoke(nameof(RestartLevel), 1.5f);  //через 1,5 сек перезапус сцены
    }

    //Рестарт уровня
    private void RestartLevel()
    {
        PlayerScore.Reset();  //Обнуляет очки и жизни
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //занова запускает текущию сцену с 0
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Platform p = collision.collider.GetComponent<Platform>();  //пытается получить Platform у колайдера с которым контакт
        if (p != null) currentPlatform = p;  //если найден то сохраняем  и передаем в HandleMovement
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Platform p = collision.collider.GetComponent<Platform>(); //пытается получить Platform у колайдера с которым контакт
        if (p != null && p == currentPlatform)  //проверяет та платформа с которой сходит или нет
            currentPlatform = null;
    }

    private IEnumerator DeathVisual()  //работает по кадрам а не мгновенно
    {
        //Делаем красным
        sprite.color = new Color (1,0,0,1);

        //отскок назад и вверх
        rb.linearVelocity = new Vector2( -facing * 2f,4f);  //отталкивает в противоположную строну и силы Х и Y

        //Мигание
        float t = 0f;  //таймиер
        while (t < 1f) //длится 1 сек
        {
            t += Time.deltaTime; //увеличивает таймер

            //плавное исчезновение
            float alpha = Mathf.Lerp(1f,0f,t); //уменьшение от виден до прозраного за t
            sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,alpha);  //сохраняет все кроме alpha

            yield return null; //останавливает до следующего кадра
        }
    }


}
