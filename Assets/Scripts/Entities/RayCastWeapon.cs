using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class RayCastWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject impactEffect;
    public float impactEffectLifeTime = 2f;

    [Header("LaserBullet Settings")]
    public GameObject laserBulletPrefab;
    public int laserBulletDamage = 2;
    public float laserBulletSpeed = 10f;
    public float laserBulletLifeTime = 1f;

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public float bulletPrefabLifeTime = 3f;
    public int bulletDamage = 20;
    public float bulletFireRate = 1.5f;
    private bool isLaserBulletMode = true;
    private float nextFireTime;
    private Player player;
    private LayerMask raycastLayerMask;
    private bool canShoot = true;

    private ObjectPool<GameObject> bulletPool;
    private ObjectPool<GameObject> laserBulletPool;
    private ObjectPool<GameObject> impactEffectPool;

    private void Awake()
    {
        raycastLayerMask = ~LayerMask.GetMask("Player");
        player = GetComponent<Player>();
        // Ініціалізуємо пул об'єктів для кожного типу об'єкта
        bulletPool = new ObjectPool<GameObject>(() => Instantiate(bulletPrefab), null, DestroyObject);
        laserBulletPool = new ObjectPool<GameObject>(() => Instantiate(laserBulletPrefab), null, DestroyObject);
        impactEffectPool = new ObjectPool<GameObject>(() => Instantiate(impactEffect), null, DestroyObject);
    }

    private void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }

    void Update()
    {
        // Перевіряємо, чи змінюється тип амуніції
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
        // Створюємо об'єкт лазерної кулі з пулу
        GameObject laserBullet = Instantiate(laserBulletPrefab, firePoint.position, Quaternion.Euler(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg));
        LaserBullet laserBulletScript = laserBullet.GetComponent<LaserBullet>();
        laserBulletScript.damage = laserBulletDamage;
        laserBulletScript.speed = laserBulletSpeed;

        // Перевіряємо, чи об'єкт не є null перед доступом до нього
        if (laserBullet != null)
        {
            // Виконуємо потрібні дії з об'єктом
            Destroy(laserBullet, laserBulletLifeTime);
        }
        else
        {
            // Обробка випадку, коли об'єкт вже був знищений
        }
    }

    void ShootBullet(Vector2 shootingDirection)
    {
        // Отримуємо об'єкт кулі з пулу
        GameObject bullet = bulletPool.Get();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
        // Налаштовуємо параметри кулі
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = bulletDamage;
        bullet.SetActive(true);
        // Запускаємо відлік часу для знищення кулі після певного інтервалу
        StartCoroutine(DelayedRelease(bullet, bulletPrefabLifeTime));
    }

    private IEnumerator DelayedRelease(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (obj != null)
        {
            // Визначаємо тип об'єкту і повертаємо його відповідний пул
            if (obj.CompareTag("Bullet"))
            {
                bulletPool.Release(obj);
            }
            else if (obj.CompareTag("LaserBullet"))
            {
                laserBulletPool.Release(obj);
            }
            else if (obj.CompareTag("ImpactEffect"))
            {
                impactEffectPool.Release(obj);
            }
        }
    }
}