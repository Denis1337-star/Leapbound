using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public Text scoreText;

    private void Update()
    {
        scoreText.text = $"Score: {PlayerScore.Score}";
    }
}
