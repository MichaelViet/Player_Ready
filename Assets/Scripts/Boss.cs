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
    public CanvasGroup BossSlider;
    private QuestSystem questSystem;
    private bool questActivated;
    private InventoryManager inventoryManager;
    private void Start()
    {
        anim = GetComponent<Animator>();
        healthBar.maxValue = health;
        healthBar.value = health;
        SetBossSliderVisibility(false);
        rayCastWeapon = GetComponent<RayCastWeapon>();
        questSystem = FindObjectOfType<QuestSystem>();
        questActivated = false;
        inventoryManager = FindObjectOfType<InventoryManager>();
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
            inventoryManager.arena.SetActive(false);
            if (!questActivated)
            {
                Quest activeQuest = questSystem.GetActiveQuest();
                if (activeQuest != null)
                {
                    questSystem.CompleteQuest(activeQuest);
                }

                Quest nextQuest = questSystem.GetActiveQuest();
                if (nextQuest != null)
                {
                    questSystem.StartQuest(nextQuest);
                }
                questActivated = true;
            }
        }

        if (timeBtwDamage > 0)
        {
            timeBtwDamage -= Time.deltaTime;
        }
        healthBar.value = health;
        SetBossSliderVisibility(!isDead && health > 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isDead == false)
        {
            if (timeBtwDamage <= 0)
            {
                other.GetComponent<Player>().health -= damage;
                timeBtwDamage = 1.5f;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Boss health: " + health);
        healthBar.value = health;
        Instantiate(rayCastWeapon.impactEffect, transform.position, transform.rotation);
        if (health <= 0)
        {
            isDead = true;
        }
    }

    private void SetBossSliderVisibility(bool isVisible)
    {
        BossSlider.alpha = isVisible ? 1 : 0;

        // Start the current quest when the boss appears
        if (isVisible)
        {
            Quest activeQuest = questSystem.GetActiveQuest();
            if (activeQuest != null)
            {
                questSystem.StartQuest(activeQuest);
            }
        }
    }
}
