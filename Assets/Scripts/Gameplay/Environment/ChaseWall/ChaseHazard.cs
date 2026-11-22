using UnityEngine;

public sealed class ChaseHazard : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _pointFrom;
    [SerializeField] private Transform _pointTo;
    [SerializeField] private float _speed;

    private bool _isMoving;
    private Vector2 _target;
    private void Awake()
    {
        _rigidbody.position = _pointFrom.position;
        Physics2D.SyncTransforms();
    }
    public void StartChase()
    {
        if (_isMoving)
            return;

        _rigidbody.position = _pointFrom.position;
        Physics2D.SyncTransforms();
        _target = _pointTo.position;
        _isMoving = true;
    }
    public void ResetChase()
    {
        _isMoving = false;
        _rigidbody.position = _pointFrom.position;
        Physics2D.SyncTransforms();
    }
    private void FixedUpdate()
    {
        if (!_isMoving)
            return;

        Vector2 current = _rigidbody.position;
        Vector2 next = Vector2.MoveTowards(current, _target, _speed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(next);

        if (Vector2.Distance(next, _target) > 0.05f)
            return;

        _isMoving = false;
    }
}
