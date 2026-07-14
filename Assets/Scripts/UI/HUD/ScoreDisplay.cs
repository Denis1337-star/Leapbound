using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private Text _scoreText;

    private IScoreService _scoreService;

    [Inject]
    public void Construct(IScoreService scoreService)
    {
        _scoreService = scoreService;
    }
    private void OnEnable()
    {
        _scoreService.OnScoreChanged += UpdateText;
    }
    private void OnDisable()
    {
        _scoreService.OnScoreChanged -= UpdateText;
    }
    private void UpdateText(int score)
    {
        _scoreText.text = $"Score: {score}";
    }
}
