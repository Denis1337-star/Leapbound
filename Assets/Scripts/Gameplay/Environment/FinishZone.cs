using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class FinishZone : ValidatedMonoBehaviour
{
    [SerializeField] private PauseMenu _pauseMenu;

    private IScoreService _scoreService;
    private IGameStateService _gameStateService;

    [Inject]
    public void Construct(IScoreService scoreService, IGameStateService gameStateService)
    {
        _scoreService = scoreService;
        _gameStateService = gameStateService;
    }
    protected override bool ValidateInternal()
    {
        return ValidationUtility.IsAssigned(this, _pauseMenu, nameof(_pauseMenu));
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _gameStateService.Win();

        int stars = LevelStarCalculator.Calculate(_scoreService.Score);

        Scene scene = SceneManager.GetActiveScene();
        LevelResultManager.SaveLevelResult(
            scene.name,        
            scene.buildIndex,  
            stars
        );

        _pauseMenu.TogglePause();
    }
}
