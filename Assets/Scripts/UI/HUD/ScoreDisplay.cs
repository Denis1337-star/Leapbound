using TMPro;
using UnityEngine;
using Zenject;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    private IScoreService _scoreService;

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
    }
    private void UpdateText(int score)
    {
        _scoreText.SetText("Счёт:{0}", score);
    }
}
