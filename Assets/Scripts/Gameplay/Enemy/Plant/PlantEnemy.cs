using System.Collections.Generic;
using UnityEngine;

public sealed class PlantEnemy : EnemyBase
{
    private static readonly int ShootHash = Animator.StringToHash("Shoot");
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireInterval = 1.5f;
    [SerializeField] private Vector2 _shootDirection = Vector2.left;
    [SerializeField] private PlantBullet _bulletPrefab;
    [SerializeField] private int _poolSize;
    [SerializeField] private PlantFireEffect _plantFireEffect;
    [SerializeField] private BulletBreakEffect _breakEffectPrefab;

    private float _timer;
    private List<PlantBullet> _bulletPoolList;
    protected override void Awake()
    {
        base.Awake();
        InitializePool();
    }

    private void Update()
    {
        if (IsDead)
            return;

        _timer += Time.deltaTime;
        if (_timer >= _fireInterval)
        {
            Shoot();
            _timer = 0f;
        }
    }

    private void Shoot()
    {
        if (GetBulletFromPool() == null)
            return;

        Animator.SetTrigger(ShootHash);
    }
    public void ReleaseShot()
    {
        if (IsDead)
            return;

        PlantBullet bullet = GetBulletFromPool();
        if (bullet == null)
            return;

        _plantFireEffect.Play();
        bullet.Launch(_shootDirection);
    }
    private void InitializePool()
    {
        _bulletPoolList = new List<PlantBullet>(_poolSize);

        while (_bulletPoolList.Count < _poolSize)
        {
            PlantBullet newBullet = Instantiate(_bulletPrefab, _firePoint);
            newBullet.transform.localPosition = Vector3.zero;
            newBullet.gameObject.SetActive(false);
            _bulletPoolList.Add(newBullet);
        }
        BulletBreakEffectPool breakEffectPool = new BulletBreakEffectPool(_breakEffectPrefab, _poolSize);
        for (int i = 0; i < _bulletPoolList.Count; i++)
        {
            _bulletPoolList[i].SetBreakEffectPool(breakEffectPool);
        }
    }
    private PlantBullet GetBulletFromPool()
    {
        for(int i = 0; i<_bulletPoolList.Count; i++)
        {
            PlantBullet bullet = _bulletPoolList[i];
            if (bullet != null && !bullet.gameObject.activeInHierarchy) 
                return bullet;
        }
        return null;
    }
}
