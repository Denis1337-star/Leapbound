using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelResultManager : MonoBehaviour
{
    public static int starsEarned { get; private set; }  //текущие количество звезд
    public static int score { get; private set; }  //счет текущий
     
    private static int oneStarThreshold = 50; //очков дл€ 1 звезды
    private static int twoStarThreshold = 70;
    private static int threeStarThreshold = 100;

    public static void EvaluateStars()
    {
        score = PlayerScore.CurrentScore;  //берет текущий счет 

        //оценка его в звездах
        if (score >= threeStarThreshold)
            starsEarned = 3;
        else if (score >= twoStarThreshold)
            starsEarned = 2;
        else if (score >= oneStarThreshold)
            starsEarned = 1;
        else
            starsEarned = 0;
    }

    public static void SaveResult(string levelName, int nextLevelIndex)  //лвл и и след уровень
    {
        EvaluateStars(); //проверка звезд

        string key = $"Level_{levelName}_Stars";  //ключ дл€ сохр звезд
        int previous = PlayerPrefs.GetInt(key, 0);  //число звезд

        if (starsEarned > previous)  //если текущие звезды больше предыдущего результата сохр новые
            PlayerPrefs.SetInt(key, starsEarned);

        //  –азблокировать следующий уровень
        string nextKey = $"LevelUnlocked_{nextLevelIndex}";  //ключ дл€ след уровн€
        PlayerPrefs.SetInt(nextKey, 1);  //активирует флаг разблок

        //  «апомнить последний открытый уровень
        PlayerPrefs.SetInt("LastUnlockedLevel", Mathf.Max(PlayerPrefs.GetInt("LastUnlockedLevel", 1), nextLevelIndex)); //сравнивает 

        PlayerPrefs.Save();  //сохр результат
    }

    public static int GetStarsForLevel(string levelName)
    {
        return PlayerPrefs.GetInt($"Level_{levelName}_Stars", 0);  //возращет звезды дл€ указ уровн€
    }

    public static bool IsLevelUnlocked(int index)
    {
        if (index == 1) return true; // первый уровень всегда открыт
        return PlayerPrefs.GetInt($"LevelUnlocked_{index}", 0) == 1;  //загружает значение по ключу если 1 то true
    }

    public static int GetLastUnlockedLevel()
    {
        return PlayerPrefs.GetInt("LastUnlockedLevel", 1);  //возрощает индекс послднего разблок уровн€ (1 по умолч)
    }

}
