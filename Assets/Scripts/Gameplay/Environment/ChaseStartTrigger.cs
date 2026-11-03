using UnityEngine;
using Zenject;

public sealed class ChaseStartTrigger : MonoBehaviour
{
    [SerializeField] private ChaseHazard _chaseHazard;
    private PlayerRespawn _playerRespawn;
    private bool _isStarted;

    [Inject]
    public void Construct(PlayerRespawn playerRespawn)
    {
        _playerRespawn = playerRespawn;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isStarted)
            return;

        if (!collision.TryGetComponent(out Player player))
            return;

        _isStarted = true;
        _chaseHazard.StartChase();
    }
    private void OnEnable()
    {
        _playerRespawn.OnRespawned += ResetChase;
    }
    private void OnDisable()
    {
        _playerRespawn.OnRespawned -= ResetChase;
    }
    private void ResetChase()
    {
        _isStarted = false;
        _chaseHazard.ResetChase();
    }
}
