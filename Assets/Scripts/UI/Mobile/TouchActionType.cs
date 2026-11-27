using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public enum TouchActionType
{
    Jump,
    Attack
}

public sealed class TouchActionButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TouchActionType _action;

    private GameplayInputService _input;

    [Inject]
    public void Construct(GameplayInputService input)
    {
        _input = input;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_action == TouchActionType.Jump)
            _input.BufferJump();
        else
            _input.BufferAttack();
    }
}