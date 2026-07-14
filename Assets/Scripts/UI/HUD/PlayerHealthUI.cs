using UnityEngine;
using UnityEngine.UI;

//Для отображения Интерфейса здоровья
public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Text _hpText;
    [SerializeField] private PlayerHealth _playerHealth;

    private void Start()
    {
        if (_playerHealth != null)
            UpdateText(_playerHealth.CurrentHP, _playerHealth.maxHealth);
    }
    private void OnEnable()
    {
        _playerHealth.OnHealthChange += UpdateText;
    }
    private void OnDisable()
    {
        _playerHealth.OnHealthChange -= UpdateText;
    }
    private void UpdateText(int current, int max)
    {
        _hpText.text = $"HP:{current}/{max}";
    }
}
