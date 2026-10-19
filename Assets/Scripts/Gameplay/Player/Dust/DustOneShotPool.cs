using System.Collections.Generic;
using UnityEngine;

public sealed class DustOneShotPool 
{
    private readonly List<DustOneShot> _items;
    public DustOneShotPool(DustOneShot prefab, int size)
    {
        _items = new List<DustOneShot>(size);
        for (int i = 0; i < size; i++)
        {
            DustOneShot dust = Object.Instantiate(prefab);
            dust.gameObject.SetActive(false);
            _items.Add(dust);
        }
    }
    public void Play(DustOneShotType type, Vector3 worldPosition)
    {
        for(int i = 0; i< _items.Count; i++)
        {
            if (_items[i].IsBusy)
                continue;

            _items[i].Play(type, worldPosition);
            return;
        }
    }
}
