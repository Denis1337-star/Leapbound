using System.Collections.Generic;
using UnityEngine;

public sealed class SlimeTrailPool
{
    private readonly List<SlimeTrail> _items;

    public SlimeTrailPool(SlimeTrail prefab, int size)
    {
        _items = new List<SlimeTrail>(size);
        for (int i = 0; i < size; i++)
        {
            SlimeTrail trail = Object.Instantiate(prefab);
            trail.Stop();
            _items.Add(trail);
        }
    }
    public void Play(Vector3 worldPosition)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].IsBusy)
                continue;
            _items[i].Play(worldPosition);
            return;
        }
    }
}