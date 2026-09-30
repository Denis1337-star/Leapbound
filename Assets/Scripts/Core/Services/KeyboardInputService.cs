using UnityEngine;
using Zenject;

public class KeyboardInputService : IInputService, ITickable
{
    public float Move { get; private set; }
    public bool RunHeld { get; private set; }
    public bool CrouchHeld { get; private set; }
    public bool JumpPressed { get; private set; }
    public void Tick()
    {
        Move = Input.GetAxisRaw("Horizontal");
        RunHeld = Input.GetKey(KeyCode.LeftShift);
        CrouchHeld = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        JumpPressed = Input.GetButtonDown("Jump");
    }
    public void ConsumeJump()
    {
        JumpPressed = false;
    }
}
