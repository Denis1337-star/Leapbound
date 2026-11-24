using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class LeaderboardEntryView : MonoBehaviour
{
    [SerializeField] private TMP_Text _rankText;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _valueText;
    [SerializeField] private Image _rowBackground;
    [SerializeField] private Color _highlightAsMeColor = new Color(0.35f, 0.82f, 0.38f, 1f);

    public void Set(string playerName, string value, bool isMe)
    {
        _nameText.SetText(playerName);
        _valueText.SetText(value);
        SetMeHighlight(isMe);
    }

    public void SetMyPlace(int place, string value)
    {
        if (_rankText != null)
            _rankText.SetText("#{0}", place);

        _nameText.SetText("Я");
        _valueText.SetText(value);
        SetMeHighlight(true);
    }

    private void SetMeHighlight(bool isMe)
    {
        if (_rowBackground == null)
            return;

        if (isMe)
            _rowBackground.color = _highlightAsMeColor;
        else
            _rowBackground.color = Color.white;
    }
}