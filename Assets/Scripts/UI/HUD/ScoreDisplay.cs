using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public sealed class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private RectTransform _flyTarget;
    [SerializeField] private float _punchScale = 0.2f;
    [SerializeField] private float _punchDuration = 0.2f;

    private IScoreService _scoreService;
    private Tween _punchTween;

    public RectTransform FlyTarget => _flyTarget;

    [Inject]
    public void Construct(IScoreService scoreService)
    {
        _scoreService = scoreService;
    }
    private void OnEnable()
    {
        _scoreService.OnScoreChanged += UpdateText;
        UpdateText(_scoreService.Score);
    }
    private void OnDisable()
    {
        _scoreService.OnScoreChanged -= UpdateText;
        KillPunchTween();
    }
    private void OnDestroy()
    {
        KillPunchTween();
    }
    private void UpdateText(int score)
    {
        _scoreText.SetText("{0}", score);
        PlayPunch();
    }
    private void PlayPunch()
    {
        KillPunchTween();
        _scoreText.rectTransform.localScale = Vector3.one;
        _punchTween = _scoreText.rectTransform
            .DOPunchScale(Vector3.one * _punchScale, _punchDuration, 6, 0.5f)
            .SetUpdate(true);
    }
    private void KillPunchTween()
    {
        if (_punchTween == null)
            return;

        _punchTween.Kill();
        _punchTween = null;
    }
}