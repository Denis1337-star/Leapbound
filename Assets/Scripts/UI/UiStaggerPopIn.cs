using DG.Tweening;
using UnityEngine;

public sealed class UiStaggerPopIn : MonoBehaviour
{
    [SerializeField] private RectTransform[] _targets;
    [SerializeField] private float _startScale = 0.85f;
    [SerializeField] private float _popDuration = 0.25f;
    [SerializeField] private float _delayBetween = 0.08f;
    [SerializeField] private float _startDelay = 0.05f;

    private readonly Tween[] _tweens = new Tween[16];

    private void OnEnable() { Play(); }
    private void OnDisable()
    {
        KillAll();
        ResetScales();
    }
    private void OnDestroy() { KillAll(); }
    private void Play()
    {
        KillAll();
        int count = _targets.Length;
        for (int i = 0; i < count; i++)
        {
            RectTransform target = _targets[i];
            if (target == null)
                continue;

            target.localScale = Vector3.one * _startScale;
            float delay = _startDelay + _delayBetween * i;
            _tweens[i] = target
                .DOScale(1f, _popDuration)
                .SetDelay(delay)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
        }
    }
    private void ResetScales()
    {
        for (int i = 0; i < _targets.Length; i++)
        {
            if (_targets[i] == null)
                continue;
            _targets[i].localScale = Vector3.one;
        }
    }
    private void KillAll()
    {
        for (int i = 0; i < _tweens.Length; i++)
        {
            if (_tweens[i] == null)
                continue;
            _tweens[i].Kill();
            _tweens[i] = null;
        }
    }
}