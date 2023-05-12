using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public int health;
    public int damage;
    private float timeBtwDamage = 1.5f;
    public Slider healthBar;
    private Animator anim;
    public bool isDead;
    private RayCastWeapon rayCastWeapon;
    private void Start()
    {
        anim = GetComponent<Animator>();
        healthBar.maxValue = health;
        healthBar.value = health;
        rayCastWeapon = GameObject.FindGameObjectWithTag("Player").GetComponent<RayCastWeapon>();
    }

    private void Update()
    {

        if (health <= 300)
        {
            anim.SetTrigger("stageTwo");
            damage = 35;
        }

        if (health <= 0)
        {
            anim.SetTrigger("death");
            isDead = true;
        }

        // give the player some time to recover before taking more damage !
        if (timeBtwDamage > 0)
        {
            timeBtwDamage -= Time.deltaTime;
        }

        healthBar.value = health;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // deal the player damage ! 
        if (other.CompareTag("Player") && isDead == false)
        {
            if (timeBtwDamage <= 0)
            {
                other.GetComponent<PlayerMovement>().health -= damage;
                timeBtwDamage = 1.5f; // reset the time between damage
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Boss health: " + health); // add this line
        healthBar.value = health;
        Instantiate(rayCastWeapon.impactEffect, transform.position, transform.rotation);
        if (health <= 0)
        {
            isDead = true;
        }
    }

}
