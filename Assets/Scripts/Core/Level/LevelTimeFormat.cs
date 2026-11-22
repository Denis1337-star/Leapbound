using UnityEngine;

public static class LevelTimeFormat
{
    public static string Format(float seconds)
    {
        int total = Mathf.Max(0, Mathf.FloorToInt(seconds));
        int minutes = total / 60;
        int secs = total % 60;
        return string.Format("{0:00}:{1:00}", minutes, secs);
    }
}