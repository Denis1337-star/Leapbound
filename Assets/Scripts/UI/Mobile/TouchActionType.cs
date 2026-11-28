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
    [SerializeField] private TouchActionType _touchActionType   ;

    private GameplayInputService _gameplayInput;

    [Inject]
    public void Construct(GameplayInputService gameplayInput)
    {
        _gameplayInput = gameplayInput;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_touchActionType == TouchActionType.Jump)
            _gameplayInput.BufferJump();
        else
            _gameplayInput.BufferAttack();
    }
}