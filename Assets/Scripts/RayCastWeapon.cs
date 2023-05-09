using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject impactEffect;
    public LineRenderer lineRenderer;
    public GameObject bulletPrefab;

    [Header("Line Renderer Settings")]
    public int lineRendererDamage = 2;
    public float lineRendererFireRate = 10f;

    [Header("Bullet Settings")]
    public int bulletDamage = 20;
    public float bulletFireRate = 1.5f;

    private bool isLineRendererMode = true;
    private float nextFireTime;

    private LayerMask raycastLayerMask;

    private void Awake()
    {
        raycastLayerMask = ~LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isLineRendererMode = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isLineRendererMode = false;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Vector2 shootingDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position);
            shootingDirection.Normalize();

            if (isLineRendererMode)
            {
                nextFireTime = Time.time + 1f / lineRendererFireRate;
                StartCoroutine(ShootLineRenderer(shootingDirection));
            }
            else
            {
                nextFireTime = Time.time + 1f / bulletFireRate;
                ShootBullet(shootingDirection);
            }

        }
    }

    IEnumerator ShootLineRenderer(Vector2 shootingDirection)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, shootingDirection, Mathf.Infinity, raycastLayerMask);

        if (hitInfo)
        {
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(lineRendererDamage);
            }

            Instantiate(impactEffect, hitInfo.point, Quaternion.identity);

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, (Vector2)firePoint.position + shootingDirection * 100);
        }

        lineRenderer.enabled = true;

        yield return 0;

        lineRenderer.enabled = false;
    }

    void ShootBullet(Vector2 shootingDirection)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg));
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = bulletDamage;
    }
}
