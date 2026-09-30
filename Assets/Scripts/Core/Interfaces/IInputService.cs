using UnityEngine;

public interface IInputService 
{
    float Move { get; }
    bool RunHeld { get; }
    bool CrouchHeld { get; }
    bool JumpPressed { get; }
    void ConsumeJump();
}
