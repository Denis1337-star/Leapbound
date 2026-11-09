using System;
using YG;
using Zenject;

public sealed class GameReviewPrompt : IInitializable, IDisposable
{
    private const int WinsRequired = 3;

    public void Initialize()
    {
        YG2.onReviewSent += OnReviewSent;
    }
    public void Dispose()
    {
        YG2.onReviewSent -= OnReviewSent;
    }
    public bool CanShowSoftPrompt()
    {
        if (YG2.saves.reviewPromptShown || YG2.saves.reviewCompleted)
            return false;

        if (!YG2.reviewCanShow)
            return false;

        return YG2.saves.winCount >= WinsRequired;
    }
    public void Accept()
    {
        YG2.saves.reviewPromptShown = true;
        YG2.SaveProgress();
        if (!YG2.reviewCanShow)
            return;

        YG2.ReviewShow();
    }
    public void Decline()
    {
        YG2.saves.reviewPromptShown = true;
        YG2.SaveProgress();
    }
    private void OnReviewSent(bool complete)
    {
        if (complete)
            YG2.saves.reviewCompleted = true;

        YG2.saves.reviewPromptShown = true;
        YG2.SaveProgress();
    }
}