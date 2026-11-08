using DG.Tweening;
using UnityEngine;

public sealed class UiFloatBob : MonoBehaviour
{
    [SerializeField] private float _moveDistance = 12f;
    [SerializeField] private float _moveDuration = 1.4f;
    [SerializeField] private float _scaleTo = 1.04f;
    [SerializeField] private float _scaleDuration = 1.4f;

    private RectTransform _rect;
    private Sequence _sequence;
    private Vector2 _startPos;

    private void Awake()
    {
        _rect = (RectTransform)transform;
        _startPos = _rect.anchoredPosition;
    }
    private void OnEnable()
    {
        KillTween();
        _rect.anchoredPosition = _startPos;
        _rect.localScale = Vector3.one;

        _sequence = DOTween.Sequence().SetUpdate(true);
        _sequence.Join(
            _rect.DOAnchorPosY(_startPos.y + _moveDistance, _moveDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo));
        _sequence.Join(
            _rect.DOScale(_scaleTo, _scaleDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo));
    }
    private void OnDisable()
    {
        KillTween();
        _rect.anchoredPosition = _startPos;
        _rect.localScale = Vector3.one;
    }
    private void OnDestroy()
    {
        KillTween();
    }
    private void KillTween()
    {
        if (_sequence == null)
            return;
        _sequence.Kill();
        _sequence = null;
    }
}