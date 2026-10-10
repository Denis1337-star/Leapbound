using UnityEngine;
using Zenject;

public sealed class Coin : MonoBehaviour
{
    [SerializeField] private int _value = 10;

    private IScoreService _scoreService;
    private bool _isCollected;

    [Inject]
    public void Construct(IScoreService scoreService)
    {
    _scoreService = scoreService;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCollected)
            return;

        if (!other.TryGetComponent(out Player player))
            return;

        _isCollected = true;
        _scoreService.Add(_value);
        gameObject.SetActive(false);
    }
}
