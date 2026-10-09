using TMPro;
using UnityEngine;
using Zenject;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _hpText;

    private Player _player;
    private bool _isSubscribed;
    [Inject]
    public void Construct(Player player)
    {
        _player = player;
    }
    private void Start()
    {
        Subscribe();
    }
    private void OnEnable()
    {
        Subscribe();
    }
    private void OnDisable()
    {
        if (!_isSubscribed)
            return;

        _player.PlayerHealth.OnHealthChanged -= UpdateText;
        _isSubscribed = false;
    }
    private void Subscribe()
    {
        _player.PlayerHealth.OnHealthChanged += UpdateText;
        _isSubscribed = true;
        UpdateText(_player.PlayerHealth.CurrentHealth, _player.PlayerHealth.MaxHealth);
    }

    private void UpdateText(int current, int max)
    {
        _hpText.SetText("ХП: {0}/{1}", current, max);
    }
}
