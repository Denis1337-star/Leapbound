using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public sealed class SceneFadeView : MonoBehaviour
{
    private static readonly int RadiusId = Shader.PropertyToID("_Radius");

    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 0.35f;
    [SerializeField] private float _closedRadius = 0f;
    [SerializeField] private float _openRadius = 1.35f;

    private Material _material;
    private Tween _tween;
    private float _radius;

    private void Awake()
    {
        if (_fadeImage.material != null)
        {
            _material = Instantiate(_fadeImage.material);
            _fadeImage.material = _material;
        }
        SetOpenIdle();
    }
    public void FadeOut(Action onComplete)
    {
        KillTween();
        _fadeImage.enabled = true;
        _fadeImage.raycastTarget = true;
        _radius = _closedRadius;
        SetRadius(_radius);
        _tween = DOVirtual.DelayedCall(0.05f, () => onComplete?.Invoke())
            .SetUpdate(true);
    }
    public void FadeIn(Action onComplete = null)
    {
        KillTween();
        _fadeImage.enabled = true;
        _fadeImage.raycastTarget = true;
        _radius = _closedRadius;
        SetRadius(_radius);
        _tween = DOTween
            .To(() => _radius, value =>
            {
                _radius = value;
                SetRadius(_radius);
            }, _openRadius, _fadeDuration)
            .SetUpdate(true)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                SetOpenIdle();
                onComplete?.Invoke();
            });
    }
    private void SetOpenIdle()
    {
        KillTween();
        _radius = _openRadius;
        SetRadius(_radius);
        _fadeImage.raycastTarget = false;
        _fadeImage.enabled = false; 
    }

    private void SetRadius(float radius)
    {
        if (_material == null)
            return;
        _material.SetFloat(RadiusId, radius);
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
        if (_material != null)
            Destroy(_material);
    }
}