using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1.5f;
    public int damage = 20;
    public Rigidbody2D rb;
    public GameObject impactEffect;
    private Animator camAnim;

    // Use this for initialization
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Boss"))
        {
            camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
            camAnim.SetTrigger("shake");
            hitInfo.GetComponent<Boss>().health -= damage;
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
