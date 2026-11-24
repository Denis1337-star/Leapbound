using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class LevelPillView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _label;

    public int LevelIndex { get; private set; }

    public void Setup(int levelIndex, Action onClick)
    {
        LevelIndex = levelIndex;
        _label.SetText("{0}", levelIndex);
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => onClick());
    }

    public void SetSelected(bool selected, Color on, Color off)
    {
        if (selected)
            _button.image.color = on;
        else
            _button.image.color = off;
    }
}