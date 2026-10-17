using System;

public interface IInputService 
{
    float Move { get; }
    bool RunHeld { get; }
    bool JumpPressed { get; }
    void ConsumeJump();
    event Action PausedRequested;
}
