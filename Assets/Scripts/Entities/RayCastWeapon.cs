using System.Collections;
using UnityEngine;

public class RayCastWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject impactEffect;
    public GameObject bulletPrefab;
    public GameObject laserBulletPrefab;

    [Header("LaserBullet Settings")]
    public int laserBulletDamage = 2;
    public float laserBulletSpeed = 10f;
    public float laserBulletLifeTime = 1f;

    [Header("Bullet Settings")]
    public int bulletDamage = 20;
    public float bulletFireRate = 1.5f;
    private bool isLaserBulletMode = true;
    private float nextFireTime;
    private Player player;
    private LayerMask raycastLayerMask;
    private bool canShoot = true;

    private void Awake()
    {
        raycastLayerMask = ~LayerMask.GetMask("Player");
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isLaserBulletMode = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isLaserBulletMode = false;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && canShoot)
        {
            Vector2 shootingDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position);
            shootingDirection.Normalize();

            if (isLaserBulletMode)
            {
                nextFireTime = Time.time + 1f / laserBulletSpeed;
                ShootLaserBullet(shootingDirection);
            }
            else
            {
                nextFireTime = Time.time + 1f / bulletFireRate;
                ShootBullet(shootingDirection);
            }
        }
    }

    public void SetCanShoot(bool value)
    {
        canShoot = value;
    }

    void ShootLaserBullet(Vector2 shootingDirection)
    {
        GameObject laserBullet = Instantiate(laserBulletPrefab, firePoint.position, Quaternion.Euler(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg));
        LaserBullet laserBulletScript = laserBullet.GetComponent<LaserBullet>();
        laserBulletScript.damage = laserBulletDamage;
        laserBulletScript.speed = laserBulletSpeed;
        Destroy(laserBullet, laserBulletLifeTime);
    }

    void ShootBullet(Vector2 shootingDirection)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg));
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = bulletDamage;
    }
}

