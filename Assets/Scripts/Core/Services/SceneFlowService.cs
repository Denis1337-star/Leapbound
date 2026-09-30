using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneFlowService : ISceneFlowService
{
    private const string MainMenuSceneName = "MainMenu";

    private readonly IScoreService _scoreService;
    private readonly IGameStateService _gameStateService;

    public SceneFlowService(IScoreService scoreService, IGameStateService gameStateService)
    {
        _scoreService = scoreService;
        _gameStateService = gameStateService;
    }
    public void RestartCurrentLevel()
    {
        PrepareForSceneLoad();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMainMenu()
    {
        PrepareForSceneLoad();
        SceneManager.LoadScene(MainMenuSceneName);
    }
    public void LoadNextLevel()
    {
        PrepareForSceneLoad();

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            SceneManager.LoadScene(MainMenuSceneName);
    }
    public void LoadLevelByBuildIndex(int buildIndex)
    {
        PrepareForSceneLoad();
        SceneManager.LoadScene(buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    private void PrepareForSceneLoad()
    {
        Time.timeScale = 1.0f;
        _scoreService.Reset();
        _gameStateService.Reset();
    }
}
