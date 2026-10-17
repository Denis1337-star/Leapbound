using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public sealed class FinishZone : MonoBehaviour
{
    [SerializeField] private Collider2D _bodyCollider;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isCompleted)
            return;

        if (!collision.collider.TryGetComponent(out Player player))
            return;

        if (player.PlayerHealth.CurrentHealth <= 0)
            return;

        float playerBottom = player.BodyCollider.bounds.min.y;
        float finishTop = _bodyCollider.bounds.max.y;

        if (playerBottom <= finishTop - 0.05f)
            return;

        _isCompleted = true;
        player.FreezeBody();
        _gameStateService.Win();

        Scene scene = SceneManager.GetActiveScene();
        int stars = _levelStarCalculator.Calculate(_scoreService.Score);
        _levelResultManager.SaveLevelResult(scene.name, scene.buildIndex, stars);
    }
}
