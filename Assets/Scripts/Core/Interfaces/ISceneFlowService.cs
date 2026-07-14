public interface ISceneFlowService 
{
    void RestartCurrentLevel();
    void LoadMainMenu();
    void LoadNextLevel();
    void LoadLevelByBuildIndex(int buildIndex);
    void QuitGame();
}
