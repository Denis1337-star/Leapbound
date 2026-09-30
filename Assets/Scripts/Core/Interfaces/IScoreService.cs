using System;

public interface IScoreService 
{
    int Score { get; }
    event Action<int> OnScoreChanged;
    void Add(int amount);
    void Reset();
}
