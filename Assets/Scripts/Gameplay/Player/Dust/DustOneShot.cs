using UnityEngine;

public enum DustOneShotType
{
    Jump,
    Land
}
public class DustOneShot : MonoBehaviour
{
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int LandHash = Animator.StringToHash("Land");

    [SerializeField] private Animator _animator;
    [SerializeField] private float _jumpDuration = 0.35f;
    [SerializeField] private float _landDuration = 0.35f;
    private float _timer;
    private bool _busy;
    public bool IsBusy => _busy;
    public void Play(DustOneShotType type, Vector3 worldPosition)
    {
        transform.SetParent(null, true);
        transform.position = worldPosition;
        gameObject.SetActive(true);

        _busy = true;
        if (type == DustOneShotType.Jump)
        {
            _timer = _jumpDuration;
            _animator.ResetTrigger(LandHash);
            _animator.SetTrigger(JumpHash);
        }
        else
        {
            _timer = _landDuration;
            _animator.ResetTrigger(JumpHash);
            _animator.SetTrigger(LandHash);
        }
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
