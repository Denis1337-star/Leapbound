using UnityEngine;


//Отвечает за перемещениее игрока
[RequireComponent (typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 3f;  //Базовая скорость ходьбы
    [SerializeField] private float _runSpeed = 6f;   //Скорость бега
    [SerializeField] private float _crawlSpeedMultiplier = 0.5f; //Множитель скорости при приседание

    private Rigidbody2D _rb;
    private PlayerInput _input;  //для получения данных ввода
    private PlayerCrouch _crouch;
    private PlayerPlatformHandler _platformHandler;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _crouch = GetComponent<PlayerCrouch>();
        _platformHandler = GetComponent<PlayerPlatformHandler>();
    }
    private void FixedUpdate()
    {
        //Определяет текущую скорость:если зажата кнопка бега - runSpeed,иначе walkSpeed
        float speed = _input.RunHeld ? _runSpeed : _walkSpeed;

        //Уменьшает скорость при приседании
        if (_crouch != null && _crouch.IsCrouching)
        {
            speed *= _crawlSpeedMultiplier;
        }

        Vector2 velocity = _rb.linearVelocity;
        float platformX = _platformHandler != null  //расчет платформы
           ? _platformHandler.PlatformVelocity.x : 0f;
        velocity.x = _input.Move * speed + platformX;   //текущая скорость игрока + платформа


        _rb.linearVelocity = velocity;  //финальная 
    }
}
