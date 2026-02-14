using UnityEngine;

public class PlantEnemy : EnemyBase
{
    [Header("Shooting")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireInterval = 1.5f;
    public Vector2 shootDirection = Vector2.left;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            Shoot();
            timer = 0f;
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        bullet.GetComponent<BulletPlaint>()
              .Setup(shootDirection);
    }
}
