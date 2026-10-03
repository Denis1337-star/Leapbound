using System.Collections.Generic;
using UnityEngine;

public class PlantEnemy : EnemyBase
{
    [Header("Shooting")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireInterval = 1.5f;
    [SerializeField] private Vector2 _shootDirection = Vector2.left;
    [SerializeField] private Transform _bulletPoolRoot;
    [SerializeField] private PlantBullet _bulletPrefab;
    [SerializeField] private int _poolSize;

    private float _timer;
    private List<PlantBullet> _bulletPoolList;
    protected override void Awake()
    {
        base.Awake();
        InitializePool();
    }
    protected override bool ValidateInternal()
    {
        bool valid = true;
        valid &= ValidationUtility.IsAssigned(this, _firePoint, nameof(_firePoint));
        valid &= ValidationUtility.IsAssigned(this, _bulletPoolRoot, nameof(_bulletPoolRoot));
        valid &= ValidationUtility.IsAssigned(this, _bulletPrefab, nameof(_bulletPrefab));
        return valid;
    }

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
        PlantBullet bullet = GetBulletFromPool();
        if (bullet == null) return;

        bullet.transform.position = _firePoint.position;
        bullet.transform.rotation = Quaternion.identity;

        bullet.Setup(_shootDirection);
    }
    private void InitializePool()
    {
        _bulletPoolList = new List<PlantBullet>(_poolSize);

        while (_bulletPoolList.Count < _poolSize)
        {
            PlantBullet newBullet = Instantiate(_bulletPrefab, _bulletPoolRoot.position, Quaternion.identity, _bulletPoolRoot);
            newBullet.gameObject.SetActive(false);
            _bulletPoolList.Add(newBullet);
        }
    }
    private PlantBullet GetBulletFromPool()
    {
        for(int i = 0; i<_bulletPoolList.Count; i++)
        {
            PlantBullet bullet = _bulletPoolList[i];
            if (bullet == null) continue;
            if (!bullet.gameObject.activeInHierarchy)
                return bullet;
        }
        return _bulletPoolList[0];
    }
}
