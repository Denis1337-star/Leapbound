public static class LevelTimeLabels
{
    public static string TargetGuide(float seconds) =>
        string.Format("Уложись в {0}", LevelTimeFormat.Format(seconds));

    public static string TargetChoice(float seconds) =>
        string.Format("Цель: {0}", LevelTimeFormat.Format(seconds));

    public static string BestGuide(float seconds) =>
        string.Format("Твой рекорд: {0}", LevelTimeFormat.Format(seconds));

    public static string BestChoice(float seconds) =>
        string.Format("Лучшее: {0}", LevelTimeFormat.Format(seconds));

    public static string NoRecord => "Рекорда ещё нет";
}