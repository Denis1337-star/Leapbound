using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _pointA;  
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _speed = 2f;  
    public Vector2 Velocity { get; private set; }
    private Vector2 _target;

    private void Awake()
    {
        _target = _pointB.position;
    }

    private void FixedUpdate()
    {
        Vector2 curerentPosition = _rigidbody.position;
        Vector2 nextPosition = Vector2.MoveTowards(_rigidbody.position,
            _target, _speed * Time.fixedDeltaTime);

        Velocity = (nextPosition - curerentPosition) / Time.fixedDeltaTime;
        _rigidbody.MovePosition(nextPosition);

        if (Vector2.Distance(nextPosition, _target) > 0.05f)
            return;

        bool targetIsPointA = Vector2.Distance(_target,_pointA.position)<0.1f;
        if (targetIsPointA)
            _target = _pointB.position;
        else
            _target = _pointA.position;
    }
}

