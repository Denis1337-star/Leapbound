using UnityEngine;

public sealed class CoinFlyFxPool
{
    private readonly SimpleFxPool<CoinFlyFx> _pool;
    private readonly ScoreDisplay _scoreDisplay;

    public CoinFlyFxPool(CoinFlyFx prefab, int size, Canvas fxCanvas, ScoreDisplay scoreDisplay)
    {
        _scoreDisplay = scoreDisplay;
        _pool = new SimpleFxPool<CoinFlyFx>(prefab, size,
            item => item.IsBusy,
            item =>
            {
                item.transform.SetParent(fxCanvas.transform, false);
                item.Setup(fxCanvas);
                item.gameObject.SetActive(false);
            });
    }

    public void Play(Vector3 worldPosition)
    {
        if (_pool.TryGetFree(out CoinFlyFx item))
            item.Play(worldPosition, _scoreDisplay.FlyTarget);
    }
}