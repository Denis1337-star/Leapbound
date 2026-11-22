using UnityEngine;

public sealed class DustOneShotPool
{
    private readonly SimpleFxPool<DustOneShot> _pool;

    public DustOneShotPool(DustOneShot prefab, int size)
    {
        _pool = new SimpleFxPool<DustOneShot>(prefab, size,
            item => item.IsBusy,
            item => item.gameObject.SetActive(false));
    }

    public void Play(DustOneShotType type, Vector3 worldPosition)
    {
        if (_pool.TryGetFree(out DustOneShot item))
            item.Play(type, worldPosition);
    }
}