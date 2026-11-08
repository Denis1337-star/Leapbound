using UnityEngine;
using Zenject;

public sealed class Coin : MonoBehaviour
{
    [SerializeField] private int _value = 10;
    private CoinOneShotPool _coinOneShotPool;
    private CoinFlyFxPool _coinFlyFxPool;
    private IScoreService _scoreService;
    private bool _isCollected;

    [Inject]
    public void Construct(IScoreService scoreService, CoinOneShotPool coinOneShotPool, CoinFlyFxPool coinFlyFxPool)
    {
        _scoreService = scoreService;
        _coinOneShotPool = coinOneShotPool;
        _coinFlyFxPool = coinFlyFxPool;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCollected)
            return;

        if (!other.TryGetComponent(out Player player))
            return;

        _isCollected = true;
        Vector3 collectPosition = transform.position;
        _scoreService.Add(_value);
        _coinOneShotPool.Play(collectPosition);
        _coinFlyFxPool.Play(collectPosition);
        Destroy(gameObject);
    }
}