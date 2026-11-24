using TMPro;
using UnityEngine;
using Zenject;

public sealed class TimeTrialHud : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;

    private LevelTimer _levelTimer;

    [Inject]
    public void Construct(LevelTimer levelTimer)
    {
        _levelTimer = levelTimer;
    }

    private void OnEnable()
    {
        bool show = _levelTimer.IsTimeTrial;
        gameObject.SetActive(show);
        if (!show)
            return;

        _levelTimer.OnTimeChanged += HandleTimeChanged;
        HandleTimeChanged(_levelTimer.ElapsedSeconds, _levelTimer.TargetSeconds);
    }

    private void OnDisable()
    {
            _levelTimer.OnTimeChanged -= HandleTimeChanged;
    }

    private void HandleTimeChanged(float elapsed, float target)
    {
        _timerText.SetText(LevelTimeLabels.ElapsedOverTarget(elapsed, target));
    }
}