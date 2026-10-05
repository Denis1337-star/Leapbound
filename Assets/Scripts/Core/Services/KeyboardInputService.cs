using System;
using UnityEngine;
using Zenject;

public class KeyboardInputService : IInputService, ITickable
{
    public float Move { get; private set; }
    public bool RunHeld { get; private set; }
    public bool CrouchHeld { get; private set; }
    public bool JumpPressed { get; private set; }
    public event Action PausedRequested;
    public void Tick()
    {
        Move = Input.GetAxisRaw("Horizontal");
        RunHeld = Input.GetKey(KeyCode.LeftShift);
        CrouchHeld = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        JumpPressed = Input.GetButtonDown("Jump");
        if (Input.GetKeyDown(KeyCode.Escape))
            PausedRequested?.Invoke();
    }
    public void ConsumeJump()
    {
        JumpPressed = false;
    }
}
