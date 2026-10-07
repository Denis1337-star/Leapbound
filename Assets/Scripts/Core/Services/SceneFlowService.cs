using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneFlowService : ISceneFlowService
{
    private const string MainMenuSceneName = "MainMenu";

    public void RestartCurrentLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuSceneName);
    }
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            SceneManager.LoadScene(MainMenuSceneName);
    }
    public void LoadLevelByBuildIndex(int buildIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
