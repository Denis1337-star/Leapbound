using UnityEngine;
using YG;

public static class YandexLanguageFallback
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        YG2.onCorrectLang += OnCorrectLang;
    }
    private static void OnCorrectLang(string lang)
    {
        if (lang == "ru")
            return;

        YG2.lang = "ru";
    }
}