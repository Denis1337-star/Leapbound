using System;
using UnityEngine;
using Zenject;

public sealed class GameplayInputService : IInputService, ITickable
{
    public float Move { get; private set; }
    public bool RunHeld { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool AttackPressed { get; private set; }
    public event Action PausedRequested;

    private bool _stickHeld;
    private float _touchMove;
    private bool _touchRun;
    private bool _jumpBuffered;
    private bool _attackBuffered;

    public void Tick()
    {
        float keyboardMove = Input.GetAxisRaw("Horizontal");
        bool keyboardRun = Input.GetKey(KeyCode.LeftShift);
        bool keyboardJump = Input.GetButtonDown("Jump");
        bool keyboardAttack = Input.GetKeyDown(KeyCode.J);

        if (_stickHeld)
        {
            Move = _touchMove;
            RunHeld = _touchRun;
        }
        else
        {
            Move = keyboardMove;
            RunHeld = keyboardRun;
        }

        JumpPressed = false;
        if (_jumpBuffered || keyboardJump)
            JumpPressed = true;
        _jumpBuffered = false;

        AttackPressed = false;
        if (_attackBuffered || keyboardAttack)
            AttackPressed = true;
        _attackBuffered = false;

        if (Input.GetKeyDown(KeyCode.Escape))
            PausedRequested?.Invoke();
    }

    public void SetStick(float move, bool runHeld)
    {
        _stickHeld = true;
        _touchMove = move;
        _touchRun = runHeld;
    }

    public void ClearStick()
    {
        _stickHeld = false;
        _touchMove = 0f;
        _touchRun = false;
    }

    public void BufferJump() { _jumpBuffered = true; }
    public void BufferAttack() { _attackBuffered = true; }

    public void ConsumeJump() { JumpPressed = false; }
    public void ConsumeAttack() { AttackPressed = false; }
}