using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishZone : MonoBehaviour
{
    public string nextSceneName;  //название следующей сцены
    public int nextLevelIndex = 2; // указываем индекс следующего уровня

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;  //если косается с игроком продолжаем\нет ничего не делает

        PlayerScore.Win();  //вызывает панель 
        LevelResultManager.SaveResult(SceneManager.GetActiveScene().name, nextLevelIndex); //Сохраняет данные(активная сцена и ее индекс)

        var pm = FindAnyObjectByType<PauseMenu>();  //ищет любой обьект с компонентом pauseMenu
        if (pm) pm.TogglePause();  //если обьект найден то вызывает панель 
    }
}
