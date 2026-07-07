using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 7f; //Сила прыжка

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck; //Точка из которой производится проверка
    [SerializeField] private float _groundRadius = 0.2f; //Радус для проверки пересечения с землей
    [SerializeField] private LayerMask _groundLayer;  //Слой который считается землей

    private Rigidbody2D _rb;
    private PlayerInput _input;

    private bool _isGrounded; //Флаг находится ли игрок на земле

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
    }
    private void Update()
    {
        //Проверяет, касается ли игрок земли
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundRadius,_groundLayer);

        if (_input.JumpPressed && _isGrounded)  //Условия для прыжка
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce); //Даем скорость
            
            _input.ConsumeJump(); //Сбрасываем флаг прыжка
        }
    }
}
