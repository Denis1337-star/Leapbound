using UnityEngine;

public sealed class FanWind : MonoBehaviour
{
    private static readonly int IsOnHash = Animator.StringToHash("IsOn");
    [SerializeField] private float _force;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _windVfx;
    [SerializeField] private bool _startsOn = true;
    private bool _isOn;
    private Player _playerInZone;
    private void Awake()
    {
        SetOn(_startsOn);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Player player))
            return;

        _playerInZone = player;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (_playerInZone == null)
            return;

        if (!other.TryGetComponent(out Player player))
            return;

        if (player != _playerInZone)
            return;

        _playerInZone.ClearWindVelocity();
        _playerInZone = null;
    }
    private void FixedUpdate()
    {
        if (!_isOn || _playerInZone == null)
            return;

        if (_playerInZone.PlayerHealth.CurrentHealth <= 0)
            return;

        Vector2 wind = (Vector2)transform.up * _force;
        _playerInZone.SetWindVelocity(wind);
    }
    public void SetOn(bool isOn)
    {
        _isOn = isOn;
        _animator.SetBool(IsOnHash, isOn);

        if (isOn)
            _windVfx.Play();
        else
            _windVfx.Stop();
    }
}
