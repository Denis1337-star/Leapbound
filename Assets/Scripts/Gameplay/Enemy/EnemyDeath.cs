using System.Collections;
using UnityEngine;

public sealed class EnemyDeath 
{
    private const float PopForce = 7f;
    private const float GravityScale = 2f;
    private const float LifeTime = 2f;
    private const float SideForce = 4f;
    private readonly MonoBehaviour _host;
    private readonly SpriteRenderer _spriteRenderer;
    private readonly Collider2D _collider;
    private readonly Animator _animator;
    private readonly Rigidbody2D _rigidbody;

    public EnemyDeath(MonoBehaviour host, SpriteRenderer sprite, Collider2D collider, Animator animator, Rigidbody2D rigidbody)
    {
        _host = host;
        _spriteRenderer = sprite;
        _collider = collider;
        _animator = animator;
        _rigidbody = rigidbody;
    }
    public void Begin(Vector2 hitDirection)
    {
        _collider.enabled = false;
        _animator.enabled = false;
        _spriteRenderer.flipY = true;

        float side = Mathf.Sign(hitDirection.x);
        if (Mathf.Abs(side) < 0.1f)
            side = 1f;

        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rigidbody.gravityScale = GravityScale;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.linearVelocity = new Vector2(side * SideForce, PopForce);

        _host.StartCoroutine(DisableLater());
    }

    private IEnumerator DisableLater()
    {
        yield return new WaitForSeconds(LifeTime);
        _host.gameObject.SetActive(false);
    }
}
