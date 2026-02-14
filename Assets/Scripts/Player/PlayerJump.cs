using UnityEditorInternal;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 7f; //Сила прыжка

    [Header("Ground Check")]
    public Transform groundCheck; //Точка из которой производится проверка
    public float groundRadius = 0.2f; //Радус для проверки пересечения с землей
    public LayerMask groundLayer;  //Слой который считается землей

    private Rigidbody2D rb;
    private PlayerInput input;

    private bool isGrounded; //Флаг находится ли игрок на земле

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
    }
    private void Update()
    {
        //Проверяет, касается ли игрок земли
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius,groundLayer);

        if (input.JumpPressed && isGrounded)  //Условия для прыжка
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); //Даем скорость
            
            input.ConsumeJump(); //Сбрасываем флаг прыжка
        }
    }
}
