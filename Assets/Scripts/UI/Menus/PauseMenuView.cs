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
    [SerializeField] private Image[] _stars;
    [SerializeField] private GameObject _reviewPrompt;
    [SerializeField] private Button _reviewAcceptButton;
    [SerializeField] private Button _reviewDeclineButton;

    [SerializeField] private float _statusPunchScale = 0.20f;
    [SerializeField] private float _statusPunchDuration = 0.3f;
    [SerializeField] private float _rewardPulseScale = 1.05f;
    [SerializeField] private float _rewardPulseDuration = 0.55f;

    private RectTransform _pausePanelRect;
    private RectTransform _rewardLifeRect;
    private Tween _statusTween;
    private Tween _rewardPulseTween;

    public Button RestartButton => _restartButton;
    public Button MainMenuButton => _mainMenuButton;
    public Button NextLevelButton => _nextLevelButton;
    public Button RewardLifeButton => _rewardLifeButton;
    public Button CloseButton => _closeButton;
    public Button ReviewAcceptButton => _reviewAcceptButton;
    public Button ReviewDeclineButton => _reviewDeclineButton;

    private void Awake()
    {
        _pausePanelRect = _pausePanel.GetComponent<RectTransform>();
        _rewardLifeRect = _rewardLifeButton.transform as RectTransform;
    }
    private void OnDisable() { KillAllPolishTweens(); }
    private void OnDestroy() { KillAllPolishTweens(); }
    public void SetScore(int score) { _scoreText.SetText("{0}", score); }
    public void SetStatus(string text) { _statusText.SetText(text); }
    public void ShowStars(int count)
    {
        for (int i = 0; i < _stars.Length; i++)
            _stars[i].enabled = i < count;
    }
    public void ShowWin()
    {
        StopRewardPulse();
        SetStatus("Ты победил");
        _pausePanel.SetActive(true);   
        SetCloseButtonVisible(false);
        SetNextVisible(true);
        SetRewardVisible(false);
        SetReviewPromptVisible(false);
    }
    public void ShowLose()
    {
        SetStatus("Попытки закончились");
        ShowStars(0);
        _pausePanel.SetActive(true);   
        PlayStatusPunch();             
        SetCloseButtonVisible(false);
        SetNextVisible(false);
        SetRewardVisible(true);
        StartRewardPulse();
        SetReviewPromptVisible(false);
    }
    public void ShowPaused()
    {
        StopRewardPulse();
        SetStatus("Пауза");
        _pausePanel.SetActive(true);
        SetCloseButtonVisible(true);
        SetNextVisible(false);
        SetRewardVisible(false);
        SetReviewPromptVisible(false);
    }
    public void ShowPlaying()
    {
        StopRewardPulse();
        SetStatus(string.Empty);
        _pausePanel.SetActive(false);
        SetCloseButtonVisible(false);
        SetNextVisible(false);
        SetRewardVisible(false);
        SetReviewPromptVisible(false);
    }
    public void StopRewardPulsePublic() { StopRewardPulse(); }
    private void SetCloseButtonVisible(bool visible)
    {
        if (_closeButton == null)
            return;
        _closeButton.gameObject.SetActive(visible);
    }
    private void SetNextVisible(bool visible)
    {
        _nextLevelButton.gameObject.SetActive(visible);
    }
    private void SetRewardVisible(bool visible)
    {
        _rewardLifeButton.gameObject.SetActive(visible);
    }

    private void PlayStatusPunch()
    {
        KillStatusTween();
        _statusText.rectTransform.localScale = Vector3.one;
        _statusTween = _statusText.rectTransform
            .DOPunchScale(Vector3.one * _statusPunchScale, _statusPunchDuration, 6, 0.5f)
            .SetUpdate(true);
    }
    private void StartRewardPulse()
    {
        StopRewardPulse();
        if (_rewardLifeRect == null)
            return;

        _rewardLifeRect.localScale = Vector3.one;
        _rewardPulseTween = _rewardLifeRect
            .DOScale(_rewardPulseScale, _rewardPulseDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true);
    }
    private void StopRewardPulse()
    {
        if (_rewardPulseTween != null)
        {
            _rewardPulseTween.Kill();
            _rewardPulseTween = null;
        }
        if (_rewardLifeRect != null)
            _rewardLifeRect.localScale = Vector3.one;
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
        StopRewardPulse();
    }
    public void SetReviewPromptVisible(bool visible)
    {
        _reviewPrompt.SetActive(visible);
    }
}