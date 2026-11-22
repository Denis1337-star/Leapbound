public sealed class LevelEndPresenter
{
    private readonly LevelRunContext _levelRunContext;
    private readonly LevelTimer _levelTimer;
    private readonly LevelStarCalculator _levelStarCalculator;
    private readonly IScoreService _scoreService;
    private readonly PlayerLives _playerLives;
    private readonly PauseMenuView _view;
    private readonly GameReviewPrompt _gameReviewPrompt;
    private readonly ISceneFlowService _sceneFlowService;

    public LevelEndPresenter(LevelRunContext levelRunContext, LevelTimer levelTimer, 
        LevelStarCalculator levelStarCalculator, IScoreService scoreService, 
        PlayerLives playerLives, PauseMenuView view, GameReviewPrompt gameReviewPrompt, ISceneFlowService sceneFlowService)
    {
        _levelRunContext = levelRunContext;
        _levelTimer = levelTimer;
        _levelStarCalculator = levelStarCalculator;
        _scoreService = scoreService;
        _playerLives = playerLives;
        _view = view;
        _gameReviewPrompt = gameReviewPrompt;
        _sceneFlowService = sceneFlowService;
    }

    public void Apply(GameState newState, System.Action<bool> setFrozen)
    {
        switch (newState)
        {
            case GameState.Win:
                if (_levelRunContext.Mode == LevelRunMode.TimeTrial)
                {
                    bool success = _levelTimer.ElapsedSeconds <= _levelTimer.TargetSeconds;
                    _view.ShowTimeTrialResult(
                        _levelTimer.ElapsedSeconds, _levelTimer.TargetSeconds, success, _sceneFlowService.IsLastLevel());
                }
                else
                {
                    bool isLast = _sceneFlowService.IsLastLevel();
                    RefreshStars(true);
                    _view.ShowWin(isLast);
                    _view.SetReviewPromptVisible(_gameReviewPrompt.CanShowSoftPrompt());
                }
                setFrozen(true);
                break;
            case GameState.Lose:
                if (_levelRunContext.Mode != LevelRunMode.TimeTrial)
                    RefreshStars(false);
                _view.ShowLose();
                setFrozen(true);
                break;
            case GameState.Paused:
                if (_levelRunContext.Mode != LevelRunMode.TimeTrial)
                    RefreshStars(false);
                _view.ShowPaused();
                setFrozen(true);
                break;
            case GameState.Playing:
                _view.ShowPlaying();
                setFrozen(false);
                break;
        }
    }

    private void RefreshStars(bool finished)
    {
        LevelStarResult result = _levelStarCalculator.Calculate(
            _scoreService.Score, _playerLives.HasLostLife, finished);
        _view.ApplyStars(result, _scoreService.Score, _levelStarCalculator.TargetScore, finished);
    }
}