using System;

public interface IInputService 
{
    float Move { get; }
    bool RunHeld { get; }
    bool JumpPressed { get; }
    bool AttackPressed { get; }
    void ConsumeJump();
    void ConsumeAttack();
    event Action PausedRequested;
}
