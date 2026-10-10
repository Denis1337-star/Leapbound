using System.Collections;
using UnityEngine;

public sealed class EnemyDeath 
{
    private readonly MonoBehaviour _host;
    private readonly SpriteRenderer _spriteRenderer;
    private readonly Collider2D _collider;
    private readonly Animator _animator;
    private readonly float _fadeSpeed;
    public EnemyDeath(MonoBehaviour host, SpriteRenderer sprite, Collider2D collider, Animator animator, float fadeSpeed)
    {
        _host = host;
        _spriteRenderer = sprite;
        _collider = collider;
        _animator = animator;
        _fadeSpeed = fadeSpeed;
    }
    public void Begin()
    {
        _collider.enabled = false;
        _animator.enabled = false;

       _host.StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Color color = _spriteRenderer.color;
        float alpha = color.a;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * _fadeSpeed;
            color.a = alpha;
            _spriteRenderer.color = color;
            yield return null;
        }
       _host.gameObject.SetActive(false);
    }
}
