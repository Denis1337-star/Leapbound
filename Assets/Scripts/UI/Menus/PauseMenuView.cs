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

    private void OnDisable() { KillAllPolishTweens(); }
    private void OnDestroy() { KillAllPolishTweens(); }
    public void SetScore(int score) { _scoreText.SetText("{0}", score); }
    public void SetStatus(string text) { _statusText.SetText(text); }
    public void ShowWin()
    {
        SetStatus("Ты победил");
        _pausePanel.SetActive(true);
        SetCloseButtonVisible(false);
        SetNextVisible(true);
        SetRewardVisible(false);
        SetReviewPromptVisible(false);
        SetTimeTrialResultVisible(false);
        if (_crownIcon != null)
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
        if (_crownIcon != null)
            _crownIcon.SetActive(false);
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
        if (_crownIcon != null)
            _crownIcon.SetActive(false);
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
        if (_crownIcon != null)
            _crownIcon.SetActive(false);
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
    public void StopRewardPulsePublic()
    {
        if (_rewardLifePulse != null)
            _rewardLifePulse.Stop();
    }
    public void ShowTimeTrialResult(float elapsedSeconds, float targetSeconds, bool success)
    {
        _starView.Hide();
        SetTimeTrialResultVisible(true);
        _timeResultLabelText.SetText(string.Format("Время {0} / {1}",
            LevelTimeFormat.Format(elapsedSeconds),
            LevelTimeFormat.Format(targetSeconds)));

        if (_crownIcon != null)
            _crownIcon.SetActive(success);

        if (success)
        {
            SetStatus("Корона твоя");
            _timeResultBackground.color = _timeSuccessColor;
        }
        else
        {
            SetStatus("Не успел!");
          _timeResultBackground.color = _timeFailColor;
        }
        _pausePanel.SetActive(true);
        SetCloseButtonVisible(false);
        SetNextVisible(success);
        SetRewardVisible(false);
        SetReviewPromptVisible(false);
    }

    private void SetTimeTrialResultVisible(bool visible)
    {
        if (_timeTrialResultRoot != null)
            _timeTrialResultRoot.SetActive(visible);
    }
}