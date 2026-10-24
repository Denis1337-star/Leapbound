using UnityEngine;

public sealed class PlantFireEffect : MonoBehaviour
{
    private static readonly int FireHash = Animator.StringToHash("FireEffect");
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _duration = 0.3f;
    private float _timer;
    private bool _busy;
    private void Awake()
    {
        Hide();
    }
    public void Play()
    {
        _spriteRenderer.enabled = true;
        _animator.enabled = true;

        _busy = true;
        _timer = _duration;
        _animator.Play(FireHash, 0, 0f);
    }
    private void Update()
    {
        if (!_busy)
            return;

        _timer -= Time.deltaTime;
        if (_timer > 0f)
            return;

        Hide();
    }
    private void Hide()
    {
        _busy = false;
        _animator.enabled = false;
        _spriteRenderer.enabled = false;
    }
}
