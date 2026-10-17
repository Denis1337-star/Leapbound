using UnityEngine;

public sealed class Trampoline : MonoBehaviour
{
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    [SerializeField] private float _launchForce = 14f;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _bodyCollider;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent(out Player player))
            return;

        if (player.PlayerHealth.CurrentHealth <= 0)
            return;

        float playerBottom = player.BodyCollider.bounds.min.y;
        float trampolineTop = _bodyCollider.bounds.max.y;

        if (playerBottom <= trampolineTop - 0.05f)
            return;

        _animator.SetTrigger(JumpHash);

        Vector2 velocity = player.Rigidbody.linearVelocity;
        velocity.y = _launchForce;
        player.Rigidbody.linearVelocity = velocity;

    }
}
