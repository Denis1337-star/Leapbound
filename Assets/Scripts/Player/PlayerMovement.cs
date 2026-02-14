using UnityEngine;


//Отвечает за перемещениее игрока
[RequireComponent (typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3f;  //Базовая скорость ходьбы
    public float runSpeed = 6f;   //Скорость бега
    public float crawlSpeedMultiplier = 0.5f; //Множитель скорости при приседание

    private Rigidbody2D rb;
    private PlayerInput input;  //для получения данных ввода
    private PlayerCrouch crouch;
    private PlayerPlatformHandler platformHandler;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        crouch = GetComponent<PlayerCrouch>();
        platformHandler = GetComponent<PlayerPlatformHandler>();
    }
    private void FixedUpdate()
    {
        //Определяет текущую скорость:если зажата кнопка бега - runSpeed,иначе walkSpeed
        float speed = input.RunHeld ? runSpeed : walkSpeed;

        //Уменьшает скорость при приседании
        if (crouch != null && crouch.IsCrouching)
        {
            speed *= crawlSpeedMultiplier;
        }

        Vector2 velocity = rb.linearVelocity;
        float platformX = platformHandler != null  //расчет платформы
           ? platformHandler.PlatformVelocity.x : 0f;
        velocity.x = input.Move * speed + platformX;   //текущая скорость игрока + платформа


        rb.linearVelocity = velocity;  //финальная 
    }
}
