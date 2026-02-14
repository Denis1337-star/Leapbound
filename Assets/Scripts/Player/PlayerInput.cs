using UnityEngine;


//Класс для обработки ввода от игрока
public class PlayerInput : MonoBehaviour
{ 

    //Свойство для хранения значений горизонтального перемещения(влево/вправо)
    public float Move {  get; private set; }  

    //Флаг, указывающий, была ли нажата кнопка прыжка
    public bool JumpPressed {  get; private set; }

    //Флаг,указывающий, удерживается ли кнопка бега
    public bool RunHeld {  get; private set; }

    //Флаг, указывающий, удерживается ли кнопка приседания
    public bool CrouchHeld {  get; private set; }

    private void Update()
    {
        //Получает значение оси (A\D или стрелки)
        Move = Input.GetAxisRaw("Horizontal");

        //Проверка, удерживаются ли кнопки
        RunHeld = Input.GetKey(KeyCode.LeftShift);
        CrouchHeld = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        if (Input.GetButtonDown("Jump"))
        { 
            JumpPressed = true;
        }       
    }

    //Для сброса флага прыжка после обработки
    public void ConsumeJump()
    {
        JumpPressed = false;
    }
}
