using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public sealed class SceneFadeView : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 0.2f;

    private Tween _tween;

    private void Awake()
    {
        SetAlpha(0f);
        _fadeImage.raycastTarget = false;
    }
    public void FadeOut(Action onComplete)
    {
        KillTween();
        _fadeImage.raycastTarget = true;
        _tween = _fadeImage
            .DOFade(1f, _fadeDuration)
            .SetUpdate(true)
            .SetEase(Ease.Linear)
            .OnComplete(() => onComplete?.Invoke());
    }
    public void FadeIn(Action onComplete = null)
    {
        KillTween();
        SetAlpha(1f);
        _fadeImage.raycastTarget = true;
        _tween = _fadeImage
            .DOFade(0f, _fadeDuration)
            .SetUpdate(true)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _fadeImage.raycastTarget = false;
                onComplete?.Invoke();
            });
    }
    private void SetAlpha(float alpha)
    {
        Color color = _fadeImage.color;
        color.a = alpha;
        _fadeImage.color = color;
    }
    private void KillTween()
    {
        if (_tween == null)
            return;
        _tween.Kill();
        _tween = null;
    }
    private void OnDestroy()
    {
        KillTween();
    }
}