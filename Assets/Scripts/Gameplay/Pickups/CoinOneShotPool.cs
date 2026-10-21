using System.Collections.Generic;
using UnityEngine;

public sealed class CoinOneShotPool 
{
    private readonly List<CoinOneShot> _items;
    public CoinOneShotPool(CoinOneShot prefab, int size)
    {
        _items = new List<CoinOneShot>(size);
        for(int i = 0; i<size; i++)
        {
            CoinOneShot effect = Object.Instantiate(prefab);
            effect.gameObject.SetActive(false);
            _items.Add(effect);
        }
    }
    public void Play(Vector3 worlPosition)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].Isusy)
                continue;

            _items[i].Play(worlPosition);
            return;
        }
    }
}
