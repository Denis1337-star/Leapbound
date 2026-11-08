using DG.Tweening;
using UnityEngine;

public sealed class UiFloatBob : MonoBehaviour
{
    [SerializeField] private float _moveDistance = 12f;
    [SerializeField] private float _moveDuration = 1.4f;
    [SerializeField] private float _scaleTo = 1.04f;
    [SerializeField] private float _scaleDuration = 1.4f;

    private RectTransform _rect;
    private Vector2 _startPos;
    private Tween _moveTween;
    private Tween _scaleTween;

    private void Awake()
    {
        _rect = (RectTransform)transform;
        _startPos = _rect.anchoredPosition;
    }
    private void OnEnable()
    {
        KillTweens();
        _rect.anchoredPosition = _startPos;
        _rect.localScale = Vector3.one;
        _moveTween = _rect
            .DOAnchorPosY(_startPos.y + _moveDistance, _moveDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true);

        _scaleTween = _rect
            .DOScale(_scaleTo, _scaleDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true);
    }
    private void OnDisable()
    {
        KillTweens();
        _rect.anchoredPosition = _startPos;
        _rect.localScale = Vector3.one;
    }
    private void OnDestroy()
    {
        KillTweens();
    }
    private void KillTweens()
    {
        if (_moveTween != null) { _moveTween.Kill(); _moveTween = null; }
        if (_scaleTween != null) { _scaleTween.Kill(); _scaleTween = null; }
    }
}