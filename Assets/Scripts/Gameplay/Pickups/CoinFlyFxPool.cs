using System.Collections.Generic;
using UnityEngine;

public sealed class CoinFlyFxPool
{
    private readonly List<CoinFlyFx> _items;
    private readonly ScoreDisplay _scoreDisplay;

    public CoinFlyFxPool(CoinFlyFx prefab, int size, Canvas fxCanvas, ScoreDisplay scoreDisplay)
    {
        _scoreDisplay = scoreDisplay;
        _items = new List<CoinFlyFx>(size);

        for (int i = 0; i < size; i++)
        {
            CoinFlyFx fx = Object.Instantiate(prefab, fxCanvas.transform, false);
            fx.Setup(fxCanvas);
            fx.gameObject.SetActive(false);
            _items.Add(fx);
        }
    }
    public void Play(Vector3 worldPosition)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].IsBusy)
                continue;

            _items[i].Play(worldPosition, _scoreDisplay.FlyTarget);
            return;
        }
    }
}