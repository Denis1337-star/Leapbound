using UnityEngine;

[CreateAssetMenu(fileName ="PlayerConfig",menuName ="Leapbound/Player Config")]
public sealed class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    public float WalkSpeed;
    public float RunSpeed;
    public float JumpForce;
    public int MaxJumpCount = 2;
    public float StopmBounceForce = 7f;
    [Header("Checks")]
    public float GroundCheckRadius;
    public LayerMask GroundLayer;
    [Header("Health")]
    public int MaxHealth;
    [Header("Hit")]
    public float KnockbackForce = 6f;
    public float InvincibleTime = 1f;
    public float KnockbackLockTime = 0.2f;
    [Header("Audio")]
    public AudioClip WalkStep;
    public AudioClip RunStep;
    public AudioClip JumpClip;
    public AudioClip HurtClip;
    public AudioClip DeathClip;
    [Header("Attack")]
    public float AttackIdleDuration = 0.4f;
    public float AttackWalkDuration = 0.4f;
    public float AttackRunDuration =0.35f;
    public float AttackHitStart = 0.12f;
    public float AttackHitEnd = 0.28f;
}
