using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public sealed class FinishZone : MonoBehaviour
{
    private IScoreService _scoreService;
    private IGameStateService _gameStateService;
    private LevelResultManager _levelResultManager;
    private LevelStarCalculator _levelStarCalculator;
    private bool _isCompleted;

    [Inject]
    public void Construct(IScoreService scoreService, IGameStateService gameState,
        LevelResultManager levelResultManager, LevelStarCalculator levelStarCalculator)
    {
        _scoreService = scoreService;
        _gameStateService = gameState;
        _levelResultManager = levelResultManager;
        _levelStarCalculator = levelStarCalculator;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCompleted)
            return;

        if (!other.TryGetComponent(out Player player)) 
            return;

        _isCompleted = true;
        player.FreezeBody();
        _gameStateService.Win();

        Scene scene = SceneManager.GetActiveScene();
        int stars = _levelStarCalculator.Calculate(_scoreService.Score);
        _levelResultManager.SaveLevelResult(scene.name, scene.buildIndex, stars);
    }
}
