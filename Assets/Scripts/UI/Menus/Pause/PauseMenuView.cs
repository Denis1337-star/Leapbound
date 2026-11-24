using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class PauseMenuView : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _rewardLifeButton;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private GameObject _reviewPrompt;
    [SerializeField] private Button _reviewAcceptButton;
    [SerializeField] private Button _reviewDeclineButton;
    [SerializeField] private GameObject _crownIcon;
    [SerializeField] private GameObject _timeTrialResultRoot;
    [SerializeField] private TMP_Text _timeResultLabelText;
    [SerializeField] private PauseMenuStarView _starView;
    [SerializeField] private UiRewardPulse _rewardLifePulse;
    [SerializeField] private Image _timeResultBackground;
    [SerializeField] private Color _timeSuccessColor = new Color(0.45f, 0.85f, 0.35f);
    [SerializeField] private Color _timeFailColor = new Color(0.95f, 0.45f, 0.40f);
    [SerializeField] private float _crownPopDuration = 0.35f;
    [SerializeField] private float _crownStartScale = 0.2f;
    [SerializeField] private GameObject _timeTrialOfferRoot;
    [SerializeField] private Button _timeTrialOfferButton;
    private Tween _crownTween;

    private float _statusPunchScale = 0.20f;
    private float _statusPunchDuration = 0.3f;
    private Tween _statusTween;

    public Button RestartButton => _restartButton;
    public Button MainMenuButton => _mainMenuButton;
    public Button NextLevelButton => _nextLevelButton;
    public Button RewardLifeButton => _rewardLifeButton;
    public Button CloseButton => _closeButton;
    public Button ReviewAcceptButton => _reviewAcceptButton;
    public Button ReviewDeclineButton => _reviewDeclineButton;
    public Button TimeTrialOfferButton => _timeTrialOfferButton;

    private void OnDisable() { KillAllPolishTweens(); }
    private void OnDestroy() { KillAllPolishTweens(); }
    public void SetScore(int score) { _scoreText.SetText("{0}", score); }
    public void SetStatus(string text) { _statusText.SetText(text); }
    public void ShowWin(bool isLastLevel)
    {
        SetStatus("Ты победил");
        _pausePanel.SetActive(true);
        SetCloseButtonVisible(false);
        SetNextVisible(!isLastLevel);
        SetTimeTrialOfferVisible(isLastLevel);
        SetRewardVisible(false);
        SetReviewPromptVisible(false);
        SetTimeTrialResultVisible(false);
        _crownIcon.SetActive(false);
    }
    public void ShowLose()
    {
        SetStatus("Попытки закончились");
        _pausePanel.SetActive(true);
        PlayStatusPunch();
        SetCloseButtonVisible(false);
        SetNextVisible(false);
        SetRewardVisible(true);
        SetReviewPromptVisible(false);
        SetTimeTrialResultVisible(false);
        _crownIcon.SetActive(false);
        SetTimeTrialOfferVisible(false);
    }
    public void ShowPaused()
    {
        SetStatus("Пауза");
        _pausePanel.SetActive(true);
        SetCloseButtonVisible(true);
        SetNextVisible(false);
        SetRewardVisible(false);
        SetReviewPromptVisible(false);
        SetTimeTrialResultVisible(false);
        _crownIcon.SetActive(false);
        SetTimeTrialOfferVisible(false);
    }
    public void ShowPlaying()
    {
        SetStatus(string.Empty);
        _pausePanel.SetActive(false);
        SetCloseButtonVisible(false);
        SetNextVisible(false);
        SetRewardVisible(false);
        SetReviewPromptVisible(false);
        _starView.Hide();
        SetTimeTrialResultVisible(false);
        _crownIcon.SetActive(false);
        SetTimeTrialOfferVisible(false);
    }
    private void SetCloseButtonVisible(bool visible) { _closeButton.gameObject.SetActive(visible); }
    private void SetNextVisible(bool visible) { _nextLevelButton.gameObject.SetActive(visible); }
    private void SetRewardVisible(bool visible) { _rewardLifeButton.gameObject.SetActive(visible); }

    private void PlayStatusPunch()
    {
        KillStatusTween();
        _statusText.rectTransform.localScale = Vector3.one;
        _statusTween = _statusText.rectTransform
            .DOPunchScale(Vector3.one * _statusPunchScale, _statusPunchDuration, 6, 0.5f)
            .SetUpdate(true);
    }

    private void KillStatusTween()
    {
        if (_statusTween == null)
            return;

        _statusTween.Kill();
        _statusTween = null;
    }
    private void KillAllPolishTweens()
    {
        KillStatusTween();
        KillCrownTween();
    }
    public void SetReviewPromptVisible(bool visible) { _reviewPrompt.SetActive(visible); }
    public void ApplyStars(LevelStarResult result, int score, int targetScore, bool showBigStars)
    {
        _starView.ShowChecklist(result, score, targetScore);

        if (showBigStars)
            _starView.ShowBigStars(result);
        else
            _starView.ClearBigStars();
    }
    public void StopRewardPulse()
    {
        if (_rewardLifePulse != null)
            _rewardLifePulse.Stop();
    }
    public void ShowTimeTrialResult(float elapsedSeconds, float targetSeconds, bool success, bool isLastLevel)
    {
        _starView.Hide();
        SetTimeTrialResultVisible(true);
        _timeResultLabelText.SetText(string.Format("Время {0}",
            LevelTimeLabels.ElapsedOverTarget(elapsedSeconds, targetSeconds)));

        if (success)
        {
            PlayCrownPop();
            SetStatus("Корона твоя");
            _timeResultBackground.color = _timeSuccessColor;
        }
        else
        {
            SetStatus("Не успел!");
            _timeResultBackground.color = _timeFailColor;
            HideCrown();
        }
        _pausePanel.SetActive(true);
        SetCloseButtonVisible(false);
        SetNextVisible(success && !isLastLevel);
        SetTimeTrialOfferVisible(success && isLastLevel);
        SetRewardVisible(false);
        SetReviewPromptVisible(false);
    }

    private void SetTimeTrialResultVisible(bool visible)
    {
            _timeTrialResultRoot.SetActive(visible);
    }
    private void KillCrownTween()
    {
        if (_crownTween == null)
            return;

        _crownTween.Kill();
        _crownTween = null;
    }
    private void PlayCrownPop()
    {
        KillCrownTween();
        _crownIcon.SetActive(true);
        RectTransform crownRect = (RectTransform)_crownIcon.transform;
        crownRect.localScale = Vector3.one * _crownStartScale;
        _crownTween = crownRect
            .DOScale(Vector3.one, _crownPopDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }
    private void HideCrown()
    {
        KillCrownTween();
        _crownIcon.SetActive(false);
        _crownIcon.transform.localScale = Vector3.one;
    }
    private void SetTimeTrialOfferVisible(bool visible)
    {
            _timeTrialOfferRoot.SetActive(visible);
    }
}