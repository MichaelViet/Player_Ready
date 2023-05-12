using System.Collections;
using UnityEngine;

public class RayCastWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject impactEffect;
    public GameObject bulletPrefab;
    public GameObject laserBulletPrefab;  // New

    [Header("LaserBullet Settings")]  // Changed
    public int laserBulletDamage = 2;  // Changed
    public float laserBulletSpeed = 10f;  // Changed
    public float laserBulletLifeTime = 1f;  // New

    [Header("Bullet Settings")]
    public int bulletDamage = 20;
    public float bulletFireRate = 1.5f;
    private bool isLaserBulletMode = true;  // Changed
    private float nextFireTime;
    private PlayerMovement playerMovement;
    private LayerMask raycastLayerMask;
    private bool canShoot = true;
    private Animator camAnim;
    private void Awake()
    {
        raycastLayerMask = ~LayerMask.GetMask("Player");
        playerMovement = GetComponent<PlayerMovement>();
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isLaserBulletMode = true;  // Changed
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isLaserBulletMode = false;  // Changed
        }

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && canShoot)
        {
            Vector2 shootingDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position);
            shootingDirection.Normalize();

            if (isLaserBulletMode)  // Changed
            {
                nextFireTime = Time.time + 1f / laserBulletSpeed;  // Changed
                ShootLaserBullet(shootingDirection);  // Changed
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

    void ShootLaserBullet(Vector2 shootingDirection)  // New
    {
        GameObject laserBullet = Instantiate(laserBulletPrefab, firePoint.position, Quaternion.Euler(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg));
        LaserBullet laserBulletScript = laserBullet.GetComponent<LaserBullet>();
        laserBulletScript.damage = laserBulletDamage;
        laserBulletScript.speed = laserBulletSpeed;
        Destroy(laserBullet, laserBulletLifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
            camAnim.SetTrigger("shake");
            other.GetComponent<Boss>().health -= bulletDamage;
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    void ShootBullet(Vector2 shootingDirection)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg));
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = bulletDamage;
    }
}

