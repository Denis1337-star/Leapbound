using DG.Tweening;
using UnityEngine;

public sealed class UiPanelPopIn : MonoBehaviour
{
    [SerializeField] private float _showDuration = 0.25f;
    [SerializeField] private float _startScale = 0.85f;
    private RectTransform _rect;
    private Tween _tween;

    private void Awake()
    {
        _rect = (RectTransform)transform;
    }
    private void OnEnable()
    {
        KillTween();
        _rect.localScale = Vector3.one * _startScale;
        _tween = _rect
            .DOScale(1f, _showDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }
    private void OnDisable()
    {
        KillTween();
        _rect.localScale = Vector3.one;
    }
    private void OnDestroy()
    {
        KillTween();
    }
    private void KillTween()
    {
        if (_tween == null)
            return;
        _tween.Kill();
        _tween = null;
    }
}