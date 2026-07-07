using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private Text _scoreText;

    private void Awake()
    {
        UpdateText(0);
    }
    private void OnEnable()
    {
        PlayerScore.OnScoreChanged += UpdateText;
    }
    private void OnDisable()
    {
        PlayerScore.OnScoreChanged -= UpdateText;
    }
    private void UpdateText(int score)
    {
        _scoreText.text = $"Score: {PlayerScore.Score}";
    }
}
