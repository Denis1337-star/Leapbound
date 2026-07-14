using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class FinishZone : MonoBehaviour
{
    [SerializeField] private PauseMenu _pauseMenu;

    private IScoreService _scoreService;

    [Inject]
    public void Construct(IScoreService scoreService)
    {
        _scoreService = scoreService;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _scoreService.Win();

        int stars = LevelStarCalculator.Calculate(_scoreService.Score);

        Scene scene = SceneManager.GetActiveScene();
        LevelResultManager.SaveLevelResult(
            scene.name,        
            scene.buildIndex,  
            stars
        );

        if (_pauseMenu == null)
        {
            Debug.LogError($"{name} PauseMenu miss", this); return;
        }

        _pauseMenu.TogglePause();
    }
}
