using UnityEngine;

public sealed class BulletBreakEffectPool
{
    private readonly SimpleFxPool<BulletBreakEffect> _pool;

    public BulletBreakEffectPool(BulletBreakEffect prefab, int size)
    {
        _pool = new SimpleFxPool<BulletBreakEffect>(prefab, size,
            item => item.IsBusy,
            item => item.gameObject.SetActive(false));
    }

    public void Play(Vector3 worldPosition)
    {
        if (_pool.TryGetFree(out BulletBreakEffect item))
            item.Play(worldPosition);
    }
}