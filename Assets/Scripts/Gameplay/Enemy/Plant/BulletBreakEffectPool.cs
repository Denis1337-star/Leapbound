using System.Collections.Generic;
using UnityEngine;

public sealed class BulletBreakEffectPool 
{
    private readonly List<BulletBreakEffect> _items;
    public BulletBreakEffectPool(BulletBreakEffect prefab, int size)
    {
        _items = new List<BulletBreakEffect>(size);
        for (int i = 0; i < size; i++)
        {
            BulletBreakEffect effect = Object.Instantiate(prefab);
            effect.gameObject.SetActive(false);
            _items.Add(effect);
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
