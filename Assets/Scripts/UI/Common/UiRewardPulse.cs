using DG.Tweening;
using UnityEngine;

public sealed class UiRewardPulse : MonoBehaviour
{
    [SerializeField] private float _pulseScale = 1.05f;
    [SerializeField] private float _pulseDuration = 0.55f;

    private RectTransform _rect;
    private Tween _pulseTween;

    private void Awake()
    {
        _rect = (RectTransform)transform;
    }
    private void OnEnable()
    {
        Play();
    }
    private void OnDisable()
    {
        Stop();
    }
    private void OnDestroy()
    {
        Stop();
    }
    public void Play()
    {
        Stop();
        _rect.localScale = Vector3.one;
        _pulseTween = _rect
            .DOScale(_pulseScale, _pulseDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true);
    }
    public void Stop()
    {
        if (_pulseTween != null)
        {
            _pulseTween.Kill();
            _pulseTween = null;
        }
        if (_rect != null)
            _rect.localScale = Vector3.one;
    }
}