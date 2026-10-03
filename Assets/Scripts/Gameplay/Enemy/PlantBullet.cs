using UnityEngine;
using Zenject;

public class PlantBullet : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private int _damage = 50;

    private Vector2 _direction;
    private float _lifeTimer;

    public void Setup(Vector2 dir)
    {
        _direction = dir.normalized;
        _lifeTimer = _lifeTime;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);

        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0f)
            ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D colider)
    {
        if (!colider.CompareTag("Player"))
        {
            if (colider.CompareTag("Ground"))
                ReturnToPool();
            return;
        }

        var health = colider.GetComponent<PlayerHealth>();
        if (health != null)
        {
            Vector2 hitDir = (colider.transform.position - transform.position).normalized;
            health.TakeDamage(_damage, hitDir);
            ReturnToPool();
        }
    }
    private void ReturnToPool()
    {
        if (this == null)
            return;
        _direction = Vector2.zero;
        gameObject.SetActive(false);
    }
}
