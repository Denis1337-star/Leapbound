public sealed class LevelEndPresenter
{
    private readonly LevelRunContext _levelRunContext;
    private readonly LevelTimer _levelTimer;
    private readonly LevelStarCalculator _levelStarCalculator;
    private readonly IScoreService _scoreService;
    private readonly PlayerLives _playerLives;
    private readonly PauseMenuView _pauseMenuView;
    private readonly GameReviewPrompt _gameReviewPrompt;
    private readonly ISceneFlowService _sceneFlowService;
    private readonly AnalyticsService _analyticsService;
    private readonly LevelSessionStats _levelSessionStats;

    public LevelEndPresenter(LevelRunContext levelRunContext, LevelTimer levelTimer, LevelStarCalculator levelStarCalculator, 
        IScoreService scoreService, PlayerLives playerLives, PauseMenuView pauseMenuView, GameReviewPrompt gameReviewPrompt,
        ISceneFlowService sceneFlowService, AnalyticsService analyticsService, LevelSessionStats levelSessionStats)
    {
        _levelRunContext = levelRunContext;
        _levelTimer = levelTimer;
        _levelStarCalculator = levelStarCalculator;
        _scoreService = scoreService;
        _playerLives = playerLives;
        _pauseMenuView = pauseMenuView;
        _gameReviewPrompt = gameReviewPrompt;
        _sceneFlowService = sceneFlowService;
        _analyticsService = analyticsService;
        _levelSessionStats = levelSessionStats;
    }

    public void Apply(GameState newState, System.Action<bool> setFrozen)
    {
        switch (newState)
        {
            case GameState.Win:
                if (_levelRunContext.LevelMode == LevelRunMode.TimeTrial)
                {
                    bool success = _levelTimer.ElapsedSeconds <= _levelTimer.TargetSeconds;
                    _pauseMenuView.ShowTimeTrialResult(
                        _levelTimer.ElapsedSeconds, _levelTimer.TargetSeconds, success, _sceneFlowService.IsLastLevel());
                }
                else
                {
                    bool isLast = _sceneFlowService.IsLastLevel();
                    RefreshStars(true);
                    _pauseMenuView.ShowWin(isLast);
                    if (_gameReviewPrompt.CanShowSoftPrompt())
                    {
                        _pauseMenuView.SetReviewPromptVisible(true);
                        _analyticsService.ReviewShown();
                    }
                    else
                    {
                        _pauseMenuView.SetReviewPromptVisible(false);
                    }
                }
                setFrozen(true);
                break;
            case GameState.Lose:
                if (_levelRunContext.LevelMode != LevelRunMode.TimeTrial)
                    RefreshStars(false);
                _analyticsService.LevelFail(_levelSessionStats.LevelBuildIndex,AnalyticsLabels.RunMode(_levelRunContext),
                    _levelSessionStats.Deaths,_levelSessionStats.ElapsedSecondsRounded);
                _pauseMenuView.ShowLose();
                setFrozen(true);
                break;
            case GameState.Paused:
                if (_levelRunContext.LevelMode != LevelRunMode.TimeTrial)
                    RefreshStars(false);
                _pauseMenuView.ShowPaused();
                setFrozen(true);
                break;
            case GameState.Playing:
                _pauseMenuView.ShowPlaying();
                setFrozen(false);
                break;
        }
    }

    private void RefreshStars(bool finished)
    {
        LevelStarResult result = _levelStarCalculator.Calculate(
            _scoreService.Score, _playerLives.HasLostLife, finished);
        _pauseMenuView.ApplyStars(result, _scoreService.Score, _levelStarCalculator.TargetScore, finished);
    }
}