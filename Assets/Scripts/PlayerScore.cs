using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public static int CurrentScore { get; private set; }  //текущий счет 
    public static bool IsWin { get; private set; }  //если игрок победил
    public static bool IsGameOver { get; private set; }  //игра завершена (победа\поражение)

    public static event Action<int> OnScoreChanged;  //передает текущий счет(срабатывает при добавление и сбросе очков)
    public static event Action OnWin;
    public static event Action OnGameOver;

    private void Awake()
    {
        Reset();  //обнуляет счет
    }

    public static void AddPoints(int points)
    {
        if (IsGameOver) return;  //если игра  завершена выход 
        CurrentScore += points;  //добавляет очки
        OnScoreChanged?.Invoke(CurrentScore); //передает счет 
    }

    public static void Win()
    {
        if (IsGameOver) return;  //Если игра завершена
        IsWin = true;  //выиграл
        IsGameOver = true;  //завершена
        OnWin?.Invoke();
        OnGameOver?.Invoke();
    }

    public static void Lose()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        IsWin = false;
        OnGameOver?.Invoke();
    }

    public static void Reset()
    {  //сброс настроект
        CurrentScore = 0;
        IsWin = false;
        IsGameOver = false;
        OnScoreChanged?.Invoke(CurrentScore);
    }
}
