using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public sealed class PlayerLivesUI : MonoBehaviour
{
    [SerializeField] private Image[] _lifeImages;
    [SerializeField] private float _fallDistance = 80f;
    [SerializeField] private float _fallDuration = 0.35f;
    [SerializeField] private float _punchScale = 0.2f;
    [SerializeField] private float _punchDuration = 0.2f;

    private PlayerLives _playerLives;
    private bool _isSubscribed;
    private int _displayedLives;
    private Tween[] _lifeTweens;
    private Vector2[] _lifeStartPositions;

    [Inject]
    public void Construct(PlayerLives playerLives)
    {
        _playerLives = playerLives;
    }
    private void Awake()
    {
        _lifeTweens = new Tween[_lifeImages.Length];
        _lifeStartPositions = new Vector2[_lifeImages.Length];

        for (int i = 0; i < _lifeImages.Length; i++)
            _lifeStartPositions[i] = _lifeImages[i].rectTransform.anchoredPosition;
    }
    private void OnEnable() { Subscribe(); }
    private void OnDisable()
    {
        if (_isSubscribed)
        {
            _playerLives.OnLivesChanged -= HandleLivesChanged;
            _isSubscribed = false;
        }
        KillAllLifeTweens();
    }
    private void OnDestroy() { KillAllLifeTweens(); }
    private void Subscribe()
    {
        if (_isSubscribed)
            return;

        _playerLives.OnLivesChanged += HandleLivesChanged;
        _isSubscribed = true;
        _displayedLives = _playerLives.CurrentLives;
        ApplyLivesInstant(_displayedLives);
    }
    private void HandleLivesChanged(int current, int max)
    {
        int targetLives = Mathf.Clamp(current, 0, _lifeImages.Length);
        if (targetLives == _displayedLives)
            return;

        if (targetLives < _displayedLives)
            AnimateLoseLives(_displayedLives, targetLives);
        else
            AnimateGainLives(_displayedLives, targetLives);

        _displayedLives = targetLives;
    }
    private void ApplyLivesInstant(int activeCount)
    {
        for (int i = 0; i < _lifeImages.Length; i++)
        {
            RectTransform lifeRect = _lifeImages[i].rectTransform;
            lifeRect.anchoredPosition = _lifeStartPositions[i];
            lifeRect.localScale = Vector3.one;
            _lifeImages[i].color = Color.white;
            _lifeImages[i].gameObject.SetActive(i < activeCount);
        }
    }
    private void AnimateLoseLives(int fromCount, int toCount)
    {
        for (int i = fromCount - 1; i >= toCount; i--)
            PlayLifeFallAnimation(i);
    }
    private void AnimateGainLives(int fromCount, int toCount)
    {
        for (int i = fromCount; i < toCount; i++)
            PlayLifeAppearAnimation(i);
    }
    private void PlayLifeFallAnimation(int lifeIndex)
    {
        Image lifeImage = _lifeImages[lifeIndex];
        RectTransform lifeRect = lifeImage.rectTransform;
        KillLifeTween(lifeIndex);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(lifeRect.DOPunchScale(Vector3.one * _punchScale, _punchDuration, 6, 0.5f));
        sequence.Append(lifeRect.DOAnchorPosY(_lifeStartPositions[lifeIndex].y - _fallDistance, _fallDuration).SetEase(Ease.InBack));
        sequence.Join(lifeImage.DOFade(0f, _fallDuration));
        sequence.OnComplete(() =>
        {
            lifeImage.gameObject.SetActive(false);
            lifeRect.anchoredPosition = _lifeStartPositions[lifeIndex];
            lifeImage.color = Color.white;
            lifeRect.localScale = Vector3.one;
        });
        sequence.SetUpdate(true);
        _lifeTweens[lifeIndex] = sequence;
    }
    private void PlayLifeAppearAnimation(int lifeIndex)
    {
        Image lifeImage = _lifeImages[lifeIndex];
        RectTransform lifeRect = lifeImage.rectTransform;
        KillLifeTween(lifeIndex);

        lifeImage.gameObject.SetActive(true);
        lifeRect.anchoredPosition = _lifeStartPositions[lifeIndex] + new Vector2(0f, _fallDistance);
        Color color = lifeImage.color;
        color.a = 0f;
        lifeImage.color = color;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(lifeRect.DOAnchorPos(_lifeStartPositions[lifeIndex], _fallDuration).SetEase(Ease.OutBack));
        sequence.Join(lifeImage.DOFade(1f, _fallDuration));
        sequence.Append(lifeRect.DOPunchScale(Vector3.one * _punchScale, _punchDuration, 6, 0.5f));
        sequence.SetUpdate(true);
        _lifeTweens[lifeIndex] = sequence;
    }
    private void KillLifeTween(int lifeIndex)
    {
        _lifeTweens[lifeIndex].Kill();
        _lifeTweens[lifeIndex] = null;
    }
    private void KillAllLifeTweens()
    {
        for (int i = 0; i < _lifeTweens.Length; i++)
            KillLifeTween(i);
    }
}