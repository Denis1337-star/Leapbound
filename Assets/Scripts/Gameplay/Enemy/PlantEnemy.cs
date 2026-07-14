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
    protected override bool ValidateInternal()
    {
        bool valid = true;
        valid &= ValidationUtility.IsAssigned(this, _firePoint, nameof(_firePoint));
        valid &= ValidationUtility.IsAssigned(this, _bulletPrefab, nameof(_bulletPrefab));
        return valid;
    }

    private void Shoot()
    {
        GameObject bulletObject = Instantiate(
            _bulletPrefab,
            _firePoint.position,
            Quaternion.identity
        );

        PlantBullet bullet = bulletObject.GetComponent< PlantBullet >();

        bullet.Setup(_shootDirection);
    }
}
