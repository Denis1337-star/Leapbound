using UnityEngine;

[CreateAssetMenu(fileName ="PlayerConfig",menuName ="Leapbound/Player Config")]
public sealed class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    public float WalkSpeed;
    public float RunSpeed;
    public float JumpForce;
    [Header("Checks")]
    public float GroundCheckRadius;
    public LayerMask GroundLayer;
    [Header("Health")]
    public int MaxHealth;
    [Header("Hit")]
    public float KnockbackForce = 6f;
    public float InvincibleTime = 1f;
    public float KnockbackLockTime = 0.2f;
    public Color DamageColor = Color.red;
    public Color NormalColor = Color.white;
    [Header("Audio")]
    public AudioClip WalkStep;
    public AudioClip RunStep;
    public AudioClip JumpClip;
    public AudioClip HurtClip;
    public AudioClip DeathClip;
}
