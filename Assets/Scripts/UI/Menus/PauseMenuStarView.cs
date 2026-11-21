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
        SetBigStarsActive(true);
        if (_bigStars.Length > 0)
            _bigStars[0].enabled = result.FinishMet;

        if (_bigStars.Length > 1)
            _bigStars[1].enabled = result.ScoreMet;

        if (_bigStars.Length > 2)
            _bigStars[2].enabled = result.NoDeathMet;
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
}