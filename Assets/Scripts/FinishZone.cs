using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerScore.Win();

        int stars = CalculateStars(PlayerScore.Score);

        Scene scene = SceneManager.GetActiveScene();
        // сохраняем результат (ЛУЧШИЙ)
        LevelResultManager.SaveLevelResult(
            scene.name,        // имя сцены
            scene.buildIndex,  // индекс уровня
            stars
        );

        FindAnyObjectByType<PauseMenu>()?.TogglePause();
    }
    private int CalculateStars(int score)
    {
        if (score >= 100)
            return 3;

        if (score >= 50)
            return 2;

        return 1; // дошёл до финиша
    }
}
