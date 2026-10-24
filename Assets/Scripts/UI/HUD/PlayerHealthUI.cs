using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image _hpFill;

    private Player _player;
    private bool _isSubscribed;
    [Inject]
    public void Construct(Player player)
    {
        _player = player;
    }
    private void OnEnable()
    {
        Subscribe();
    }
    private void OnDisable()
    {
        if (!_isSubscribed)
            return;

        _player.PlayerHealth.OnHealthChanged -= UpdateFill;
        _isSubscribed = false;
    }
    private void Subscribe()
    {
        if (_isSubscribed)
            return;

        _player.PlayerHealth.OnHealthChanged += UpdateFill;
        _isSubscribed = true;
        UpdateFill(_player.PlayerHealth.CurrentHealth, _player.PlayerHealth.MaxHealth);
    }

    private void UpdateFill(int current, int max)
    {
        _hpFill.fillAmount = current / (float)max;
    }
}
