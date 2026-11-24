using UnityEngine;

public sealed class CoinOneShot : MonoBehaviour
{
    private static readonly int CollectHash = Animator.StringToHash("CoinEffect");
    [SerializeField] private Animator _animator;
    [SerializeField] private float _duration = 0.5f;
    private float _timer;
    private bool _IsBusy;
    public bool IsBusy => _IsBusy;
    public void Play(Vector3 worldPosition)
    {
        transform.SetParent(null, true);
        transform.position = worldPosition;
        gameObject.SetActive(true);

        _IsBusy = true;
        _timer = _duration;
        _animator.Play(CollectHash, 0, 0f);
    }
    private void Update()
    {
        if (!_IsBusy)
            return;

        _timer -= Time.deltaTime;
        if (_timer > 0f)
            return;

        _IsBusy = false;
        gameObject.SetActive(false);
    }
}
