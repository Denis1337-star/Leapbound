using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private PlayerHealth _playerHealth;


    private void Start()
    {
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
        _hpText.SetText("ХП: {0}/{1}", current, max);
    }
}
