using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image[] _heartImages;
    [SerializeField] private Sprite _fullHeartSprite;
    [SerializeField] private Sprite _emptyHeartSprite;

    private float _punchScale = 0.25f;
    private float _punchDuration = 0.2f;
    private float _swapDuration = 0.15f;
    private Player _player;
    private bool _isSubscribed;
    private int _displayedHearts;
    private Tween[] _heartTweens;

    [Inject]
    public void Construct(Player player)
    {
        _player = player;
    }
    private void Awake()
    {
        _heartTweens = new Tween[_heartImages.Length];
    }
    private void OnEnable()
    {
        Subscribe();
    }
    private void OnDisable()
    {
        if (_isSubscribed)
        {
            _player.PlayerHealth.OnHealthChanged -= HandleHealthChanged;
            _isSubscribed = false;
        }
        KillAllHeartTweens();
    }
    private void OnDestroy()
    {
        KillAllHeartTweens();
    }
    private void Subscribe()
    {
        if (_isSubscribed)
            return;

        _player.PlayerHealth.OnHealthChanged += HandleHealthChanged;
        _isSubscribed = true;
        _displayedHearts = _player.PlayerHealth.CurrentHealth;
        ApplyHeartsInstant(_displayedHearts);
    }
    private void HandleHealthChanged(int current, int max)
    {
        int targetHearts = Mathf.Clamp(current, 0, _heartImages.Length);
        if (targetHearts == _displayedHearts)
            return;

        if (targetHearts < _displayedHearts)
            AnimateLoseHearts(_displayedHearts, targetHearts);
        else
            AnimateGainHearts(_displayedHearts, targetHearts);

        _displayedHearts = targetHearts;
    }
    private void ApplyHeartsInstant(int filledCount)
    {
        for (int i = 0; i < _heartImages.Length; i++)
        {
            if (i < filledCount)
                _heartImages[i].sprite = _fullHeartSprite;
            else
                _heartImages[i].sprite = _emptyHeartSprite;

            _heartImages[i].color = Color.white;
            _heartImages[i].rectTransform.localScale = Vector3.one;
        }
    }
    private void AnimateLoseHearts(int fromCount, int toCount)
    {
        for (int i = fromCount - 1; i >= toCount; i--)
            PlayHeartEmptyAnimation(i);
    }
    private void AnimateGainHearts(int fromCount, int toCount)
    {
        for (int i = fromCount; i < toCount; i++)
            PlayHeartFillAnimation(i);
    }
    private void PlayHeartEmptyAnimation(int heartIndex)
    {
        Image heartImage = _heartImages[heartIndex];
        RectTransform heartRect = heartImage.rectTransform;
        KillHeartTween(heartIndex);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(heartRect.DOPunchScale(Vector3.one * _punchScale, _punchDuration, 8, 0.6f));
        sequence.AppendCallback(() => heartImage.sprite = _emptyHeartSprite);
        sequence.Join(heartImage.DOFade(0.35f, _swapDuration));
        sequence.Append(heartImage.DOFade(1f, _swapDuration));
        sequence.SetUpdate(true);
        _heartTweens[heartIndex] = sequence;
    }
    private void PlayHeartFillAnimation(int heartIndex)
    {
        Image heartImage = _heartImages[heartIndex];
        RectTransform heartRect = heartImage.rectTransform;
        KillHeartTween(heartIndex);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(heartRect.DOPunchScale(Vector3.one * _punchScale, _punchDuration, 8, 0.6f));
        sequence.AppendCallback(() => heartImage.sprite = _fullHeartSprite);
        sequence.Join(heartImage.DOFade(1f, _swapDuration));
        sequence.SetUpdate(true);
        _heartTweens[heartIndex] = sequence;
    }
    private void KillHeartTween(int heartIndex)
    {
        if (_heartTweens[heartIndex] == null)
            return;

        _heartTweens[heartIndex].Kill();
        _heartTweens[heartIndex] = null;
    }
    private void KillAllHeartTweens()
    {
        if (_heartTweens == null)
            return;

        for (int i = 0; i < _heartTweens.Length; i++)
            KillHeartTween(i);
    }
}
