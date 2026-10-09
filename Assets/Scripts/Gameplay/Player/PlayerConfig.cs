using UnityEngine;

[CreateAssetMenu(fileName ="PlayerConfig",menuName ="Leapbound/Player Config")]
public sealed class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    public float WalkSpeed;
    public float RunSpeed;
    public float CrawlSpeedMultiplier;
    public float JumpForce;
    [Header("Checks")]
    public float GroundCheckRadius;
    public float CeilingCheckRadius;
    public LayerMask GroundLayer;
    [Header("Collider")]
    public Vector2 StandardSize = new Vector2(1.35f, 2f);
    public Vector2 StandardOffset = new Vector2(0f, 1f);
    public Vector2 CrouchSize = new Vector2(1.35f, 2f);
    public Vector2 CrouchOffset = new Vector2(0f, 0.7f);
    [Header("Health")]
    public int MaxHealth;
    [Header("Hit")]
    public float KnockbackForce = 6f;
    public float InvincibleTime = 1f;
    public Color DamageColor = Color.red;
    public Color NormalColor = Color.white;
    [Header("Audio")]
    public AudioClip WalkStep;
    public AudioClip RunStep;
    public AudioClip JumpClip;
    public AudioClip HurtClip;
    public AudioClip DeathClip;
}
