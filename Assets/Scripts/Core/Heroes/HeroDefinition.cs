using UnityEngine;

[CreateAssetMenu(fileName = "HeroDefinition", menuName = "Leapbound/Hero Definition")]
public sealed class HeroDefinition : ScriptableObject
{
    public HeroId HeroId;
    public string DisplayName;
    [TextArea(2, 5)] public string Description;
    public Sprite MenuIcon;
    public RuntimeAnimatorController MenuIdle;
    public bool StartsUnlocked;
    public bool ComingSoon;
}