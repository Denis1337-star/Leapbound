using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public sealed class FinishZone : MonoBehaviour
{
    [SerializeField] private Collider2D _bodyCollider;
    private IScoreService _scoreService;
    private LevelResultManager _levelResultManager;
    private LevelStarCalculator _levelStarCalculator;
    private PlayerWinEffect _playerWinEffect;
    private PlayerLives _playerLives;
    private LevelRunContext _levelRunContext;
    private LevelTimer _levelTimer;
    private bool _isCompleted;

    [Inject]
    public void Construct(IScoreService scoreService, LevelResultManager levelResultManager,
        LevelStarCalculator levelStarCalculator, PlayerWinEffect playerWinEffect, PlayerLives playerLives,
        LevelRunContext levelRunContext, LevelTimer levelTimer)
    {
        _scoreService = scoreService;
        _levelResultManager = levelResultManager;
        _levelStarCalculator = levelStarCalculator;
        _playerWinEffect = playerWinEffect;
        _playerLives = playerLives;
        _levelRunContext = levelRunContext;
        _levelTimer = levelTimer;
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
        player.SnapOntoColliderTop(_bodyCollider);
        player.FreezeBody();
        _playerWinEffect.Begin();

        Scene scene = SceneManager.GetActiveScene();
        if (_levelRunContext.Mode == LevelRunMode.TimeTrial)
        {
            _levelTimer.Stop();
            _levelResultManager.SaveTimeTrialResult(
                scene.buildIndex, _levelTimer.ElapsedSeconds, _levelTimer.TargetSeconds);
        }
        else
        {
            LevelStarResult result = _levelStarCalculator.Calculate(
                _scoreService.Score, _playerLives.HasLostLife, true);
            _levelResultManager.SaveLevelResult(scene.buildIndex, result.Count);
        }
    }
}
