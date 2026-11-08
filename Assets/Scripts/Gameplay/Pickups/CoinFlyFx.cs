using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public sealed class CoinFlyFx : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private float _flyDuration = 0.45f;
    [SerializeField] private float _startPunch = 0.25f;
    [SerializeField] private float _startPunchDuration = 0.12f;

    private RectTransform _rect;
    private Canvas _canvas;
    private Camera _uiCamera;
    private Tween _tween;
    private bool _busy;

    public bool IsBusy => _busy;

    private void Awake()
    {
        _rect = (RectTransform)transform;
    }
    public void Setup(Canvas canvas)
    {
        _canvas = canvas;
        if(canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            _uiCamera = null;
        else
            _uiCamera = canvas.worldCamera;
    }
    public void Play(Vector3 worldPosition, RectTransform flyTarget)
    {
        _busy = true;
        KillTween();

        gameObject.SetActive(true);
        _rect.SetAsLastSibling();

        RectTransform canvasRect = (RectTransform)_canvas.transform;

        Vector2 startLocal = WorldToCanvasLocal(worldPosition, Camera.main, canvasRect);
        Vector2 endLocal = WorldToCanvasLocal(flyTarget.position, null, canvasRect);

        _rect.anchoredPosition = startLocal;
        _rect.localScale = Vector3.one;
        Color color = _image.color;
        color.a = 1f;
        _image.color = color;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_rect.DOPunchScale(Vector3.one * _startPunch, _startPunchDuration, 4, 0.5f));
        sequence.Append(_rect.DOAnchorPos(endLocal, _flyDuration).SetEase(Ease.InOutQuad));
        sequence.Join(_rect.DOScale(0.35f, _flyDuration));
        sequence.Join(_image.DOFade(0.35f, _flyDuration));
        sequence.OnComplete(Finish);
        sequence.SetUpdate(true);
        _tween = sequence;
    }

    private Vector2 WorldToCanvasLocal(Vector3 worldPosition, Camera camera, RectTransform canvasRect)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, worldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, screenPoint, _uiCamera, out Vector2 localPoint);

        return localPoint;
    }
    private void Finish()
    {
        _busy = false;
        KillTween();
        gameObject.SetActive(false);
    }
    private void KillTween()
    {
        if (_tween == null)
            return;

        _tween.Kill();
        _tween = null;
    }
    private void OnDisable()
    {
        KillTween();
        _busy = false;
    }
}