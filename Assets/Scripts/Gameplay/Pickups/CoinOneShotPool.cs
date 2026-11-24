using UnityEngine;

public sealed class CoinOneShotPool
{
    private readonly SimpleFxPool<CoinOneShot> _pool;
    public CoinOneShotPool(CoinOneShot prefab, int size)
    {
        _pool = new SimpleFxPool<CoinOneShot>(prefab, size,
            item => item.IsBusy,
            item => item.gameObject.SetActive(false));
    }
    public void Play(Vector3 worldPosition)
    {
        if (_pool.TryGetFree(out CoinOneShot item))
            item.Play(worldPosition);
    }
}