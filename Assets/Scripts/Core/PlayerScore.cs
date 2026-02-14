using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public static int Score { get; private set; }

    public static bool IsGameOver { get; private set; }
    public static bool IsWin { get; private set; }

    public static void Add(int amount)
    {
        Score += amount;
    }

    public static void Reset()
    {
        Score = 0;
        IsGameOver = false;
        IsWin = false;
    }

    public static void Win()
    {
        IsGameOver = true;
        IsWin = true;
    }

    public static void Lose()
    {
        IsGameOver = true;
        IsWin = false;
    }
}
