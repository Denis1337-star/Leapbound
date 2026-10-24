using UnityEngine;

public sealed class CoinOneShot : MonoBehaviour
{
    private static readonly int CollectHash = Animator.StringToHash("CoinEffect");
    [SerializeField] private Animator _animator;
    [SerializeField] private float _duration = 0.5f;
    private float _timer;
    private bool _busy;
    public bool Isusy => _busy;
    public void Play(Vector3 worlPosition)
    {
        transform.SetParent(null, true);
        transform.position = worlPosition;
        gameObject.SetActive(true);

        _busy = true;
        _timer = _duration;
        _animator.Play(CollectHash, 0, 0f);
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
