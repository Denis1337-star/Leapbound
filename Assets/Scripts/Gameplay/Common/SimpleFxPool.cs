using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class SimpleFxPool<T> where T : Component
{
    private readonly List<T> _items;
    private readonly Func<T, bool> _isBusy;

    public SimpleFxPool(T prefab, int size, Func<T, bool> isBusy, Action<T> onCreated = null)
    {
        _isBusy = isBusy;
        _items = new List<T>(size);
        for (int i = 0; i < size; i++)
        {
            T item = UnityEngine.Object.Instantiate(prefab);
            onCreated?.Invoke(item);
            _items.Add(item);
        }
    }

    public bool TryGetFree(out T item)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_isBusy(_items[i]))
                continue;
            item = _items[i];
            return true;
        }
        item = null;
        return false;
    }
}