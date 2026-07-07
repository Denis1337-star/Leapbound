using UnityEngine;

public class PlantEnemy : EnemyBase
{
    [Header("Shooting")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _fireInterval = 1.5f;
    [SerializeField] private Vector2 _shootDirection = Vector2.left;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _fireInterval)
        {
            Shoot();
            _timer = 0f;
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(
            _bulletPrefab,
            _firePoint.position,
            Quaternion.identity
        );

        bullet.GetComponent<PlantBullet>()
              .Setup(_shootDirection);
    }
}
