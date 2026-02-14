using System.Collections;
using UnityEngine;
using UnityEngine.UI;
 
//Для отображения Интерфейса здоровья
public class PlayerHealthUI : MonoBehaviour
{
    public Text hpText;  //Текст отображение хп
    private PlayerHealth health;
    private void Awake()
    {
        health = FindAnyObjectByType<PlayerHealth>();
    }

    private void OnEnable()
    {
        health.OnHealthChange += UpdateText;
    }
    private void OnDisable()
    {
        health.OnHealthChange -= UpdateText;
    }
    private void UpdateText(int current, int max)
    {
        hpText.text = $"HP:{current}/{max}";
    }
}
