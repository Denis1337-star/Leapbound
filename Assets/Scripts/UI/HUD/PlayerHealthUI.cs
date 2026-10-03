using TMPro;
using UnityEngine;

public class PlayerHealthUI : ValidatedMonoBehaviour
{
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private PlayerHealth _playerHealth;

    protected override bool ValidateInternal()
    {
        bool valid = true;
        valid &= ValidationUtility.IsAssigned(this, _hpText, nameof(_hpText));
        valid &= ValidationUtility.IsAssigned(this, _playerHealth, nameof(_playerHealth));
        return valid;
    }
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
        _hpText.text = $"HP:{current}/{max}";
    }
}
