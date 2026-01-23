using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public Text scoreText;  //текс счета
    private void Start() 
    { 
        UpdateText();
    }
    private void Update()
    { 
        UpdateText();
    }
    private void UpdateText()
    {
        scoreText.text = $"Score: {PlayerScore.CurrentScore}";  //берет счет и выводит
    }
}
