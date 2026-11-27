using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public sealed class TouchJoystick : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform _stickRoot;
    [SerializeField] private RectTransform _knob;
    [SerializeField] private TouchInputConfig _config;
    [SerializeField] private bool _hideWhenIdle = true;

    private float _radius;
    private GameplayInputService _input;
    private RectTransform _zone;

    [Inject]
    public void Construct(GameplayInputService input)
    {
        _input = input;
    }

    private void Awake()
    {
        _zone = (RectTransform)transform;
        _radius = _stickRoot.rect.width * 0.5f;
        if (_hideWhenIdle)
            _stickRoot.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Camera eventCamera = eventData.pressEventCamera;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            _zone, eventData.position, eventCamera, out Vector3 world);

        _stickRoot.position = world;
        _knob.anchoredPosition = Vector2.zero;

        if (_hideWhenIdle)
            _stickRoot.gameObject.SetActive(true);

        Apply(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Apply(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _knob.anchoredPosition = Vector2.zero;
        _input.ClearStick();

        if (_hideWhenIdle)
            _stickRoot.gameObject.SetActive(false);
    }

    private void Apply(PointerEventData eventData)
    {
        Camera eventCamera = eventData.pressEventCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _stickRoot, eventData.position, eventCamera, out Vector2 local);

        Vector2 delta = local - _stickRoot.rect.center;
        float x = delta.x;
        if (x > _radius)
            x = _radius;
        else if (x < -_radius)
            x = -_radius;

        _knob.anchoredPosition = new Vector2(x, 0f);

        float absX = Mathf.Abs(x / _radius);
        float move = 0f;
        bool runHeld = false;
        if (absX >= _config.DeadZone)
        {
            if (x > 0f)
                move = 1f;
            else
                move = -1f;

            if (absX >= _config.RunThreshold)
                runHeld = true;
        }

        _input.SetStick(move, runHeld);
    }
}