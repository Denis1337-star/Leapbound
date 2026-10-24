using UnityEngine;

public sealed class HealthOneShot : MonoBehaviour
{
    private readonly int HealHash = Animator.StringToHash("HealEffect");
    [SerializeField] private Animator _animator;
    [SerializeField] private float _duration = 0.5f;
    private float _timer;
    private bool _busy;
    public void Play()
    {
        gameObject.SetActive(true);
        _busy = true;
        _timer = _duration;
        _animator.Play(HealHash, 0, 0f);
    }
    private void Update()
    {
        if (!_busy)
            return;
        _timer -= Time.deltaTime;
        if (_timer > 0f)
            return;

        _busy = false;
        gameObject.SetActive(false);
    }
}
