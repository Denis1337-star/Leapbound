using UnityEngine;

public sealed class SlimeTrailPool
{
    private readonly SimpleFxPool<SlimeTrail> _pool;

    public SlimeTrailPool(SlimeTrail prefab, int size)
    {
        _pool = new SimpleFxPool<SlimeTrail>(prefab, size,
            item => item.IsBusy,
            item => item.Stop());
    }

    public void Play(Vector3 worldPosition)
    {
        if (_pool.TryGetFree(out SlimeTrail item))
            item.Play(worldPosition);
    }
}