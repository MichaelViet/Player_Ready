using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    public float speed = 1.5f;
    public int damage = 20;
    public Rigidbody2D rb;
    public GameObject impactEffect;
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Boss"))
        {
            hitInfo.GetComponent<Boss>().health -= damage;
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
