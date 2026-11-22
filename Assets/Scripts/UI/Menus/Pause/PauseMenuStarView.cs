using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class PauseMenuStarView : MonoBehaviour
{
    [SerializeField] private Image[] _bigStars;
    [SerializeField] private TMP_Text _finishResultLabelText;
    [SerializeField] private TMP_Text _scoreResultLabelText;
    [SerializeField] private TMP_Text _noDeathResultLabelText;
    [SerializeField] private Image _finishResultCheckIcon;
    [SerializeField] private Image _scoreResultCheckIcon;
    [SerializeField] private Image _noDeathResultCheckIcon;
    [SerializeField] private Sprite _checkSprite;
    [SerializeField] private Sprite _crossSprite;
    [SerializeField] private float _starPopDuration = 0.28f;
    [SerializeField] private float _starDelayBetween = 0.18f;
    [SerializeField] private float _starStartScale = 0.2f;
    private Sequence _starRevealSequence;

    private void OnDisable() { KillStarReveal(); }
    private void OnDestroy() { KillStarReveal(); }
    public void ShowChecklist(LevelStarResult result, int score, int targetScore)
    {
        SetStarResultsVisible(true);
        _finishResultLabelText.SetText("Финиш");
        _scoreResultLabelText.SetText("Очки: {0} / {1}", score, targetScore);
        _noDeathResultLabelText.SetText("Без потери жизней");
        SetResultIcon(_finishResultCheckIcon, result.FinishMet);
        SetResultIcon(_scoreResultCheckIcon, result.ScoreMet);
        SetResultIcon(_noDeathResultCheckIcon, result.NoDeathMet);
    }
    public void ShowBigStars(LevelStarResult result)
    {
        KillStarReveal();
        SetBigStarsActive(true);

        bool[] met = { result.FinishMet, result.ScoreMet, result.NoDeathMet };
        _starRevealSequence = DOTween.Sequence().SetUpdate(true);

        int revealIndex = 0;
        for (int i = 0; i < _bigStars.Length; i++)
        {
            if (_bigStars[i] == null)
                continue;

            Image star = _bigStars[i];
            star.enabled = met[i];
            if (!met[i])
                continue;

            RectTransform rect = star.rectTransform;
            rect.localScale = Vector3.one * _starStartScale;
            float delay = _starDelayBetween * revealIndex;
            revealIndex++;

            _starRevealSequence.Insert(delay,
                rect.DOScale(1f, _starPopDuration).SetEase(Ease.OutBack));
        }
    }

    public void ClearBigStars()
    {
        for (int i = 0; i < _bigStars.Length; i++)
        {
            if (_bigStars[i] == null)
                continue;
            _bigStars[i].enabled = false;
        }
    }
    public void Hide()
    {
        KillStarReveal();
        SetStarResultsVisible(false);
        SetBigStarsActive(false);
    }
    private void SetResultIcon(Image icon, bool met)
    {
        if (met)
            icon.sprite = _checkSprite;
        else
            icon.sprite = _crossSprite;
    }
    private void SetStarResultsVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
    private void SetBigStarsActive(bool active)
    {
        for (int i = 0; i < _bigStars.Length; i++)
        {
            if (_bigStars[i] == null)
                continue;
            _bigStars[i].gameObject.SetActive(active);
        }
    }
    private void KillStarReveal()
    {
        if (_starRevealSequence == null)
            return;

        _starRevealSequence.Kill();
        _starRevealSequence = null;
    }
}