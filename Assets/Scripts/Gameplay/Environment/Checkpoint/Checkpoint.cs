using UnityEngine;
using Zenject;

public sealed class Checkpoint : MonoBehaviour
{
    private static readonly int ActivateHash = Animator.StringToHash("Activate");
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _spawnPoint;
    private CheckpointService _checkpointService;
    private bool _isActivated;
    [Inject]
    public void Construct(CheckpointService checkpointService)
    {
        _checkpointService = checkpointService;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isActivated)
            return;

        if (!collision.TryGetComponent(out Player _))
            return;

        _isActivated = true;
        _checkpointService.SetSpawn(_spawnPoint.position);
        _animator.SetTrigger(ActivateHash);
    }
}
