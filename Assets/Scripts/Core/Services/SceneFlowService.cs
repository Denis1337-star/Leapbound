using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneFlowService : ISceneFlowService
{
    private const string MainMenuSceneName = "MainMenu";
    private readonly SceneFadeView _sceneFade;
    private bool _isLoading;

    public SceneFlowService(SceneFadeView sceneFade)
    {
        _sceneFade = sceneFade;
    }
    public void RestartCurrentLevel()
    {
        LoadWithFade(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMainMenu()
    {
        LoadWithFade(MainMenuSceneName);
    }
    public void LoadNextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            LoadWithFade(nextIndex);
        else
            LoadWithFade(MainMenuSceneName);
    }
    public void LoadLevelByBuildIndex(int buildIndex)
    {
        LoadWithFade(buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    private void LoadWithFade(int buildIndex)
    {
        if (_isLoading)
            return;

        _isLoading = true;
        Time.timeScale = 1f;
        _sceneFade.FadeOut(() =>
        {
            SceneManager.LoadScene(buildIndex);
            _sceneFade.FadeIn(() => _isLoading = false);
        });
    }
    private void LoadWithFade(string sceneName)
    {
        if (_isLoading)
            return;

        _isLoading = true;
        Time.timeScale = 1f;
        _sceneFade.FadeOut(() =>
        {
            SceneManager.LoadScene(sceneName);
            _sceneFade.FadeIn(() => _isLoading = false);
        });
    }
}