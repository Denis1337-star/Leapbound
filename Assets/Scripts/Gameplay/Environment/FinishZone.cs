using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class FinishZone : MonoBehaviour
{
    private IScoreService _scoreService;
    private IGameStateService _gameState;

    [Inject]
    public void Construct(IScoreService scoreService, IGameStateService gameState)
    {
        _scoreService = scoreService;
        _gameState = gameState;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _gameState.Win();

        int stars = LevelStarCalculator.Calculate(_scoreService.Score);

        Scene scene = SceneManager.GetActiveScene();
        LevelResultManager.SaveLevelResult(scene.name, scene.buildIndex, stars);
    }
}
