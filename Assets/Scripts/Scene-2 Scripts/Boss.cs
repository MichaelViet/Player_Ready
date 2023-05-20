using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public int health;
    public int damage;
    public Slider healthBar;
    public CanvasGroup BossSlider;
    public AudioClip nextMusic;
    public bool isDead;
    private float timeBtwDamage = 1.5f;
    private Animator anim;
    private RayCastWeapon rayCastWeapon;
    private InventoryManager inventoryManager;
    private QuestSystem questSystem;
    private WizzardController wizzardController;
    private bool questActivated = false;
    LevelManager levelManager;
    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        anim = GetComponent<Animator>();
        healthBar.maxValue = health;
        healthBar.value = health;
        SetBossSliderVisibility(false);
        rayCastWeapon = GetComponent<RayCastWeapon>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        questSystem = FindObjectOfType<QuestSystem>();
        wizzardController = FindObjectOfType<WizzardController>();

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
                if (nextMusic != null)
                {
                    levelManager.levelMusic = nextMusic; // замінюємо музику в LevelManager
                    levelManager.audioController.PlayAudio(levelManager.levelMusic, null, null); // відтворюємо нову музику
                }
                questActivated = true;
            }

            wizzardController.bossIsDead = true; // Змінюємо стан босса у WizzardController

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
        if (!isDead)
        {
            isDead = true;
        }
    }

    private void SetBossSliderVisibility(bool isVisible)
    {
        BossSlider.alpha = isVisible ? 1 : 0;
    }

    public void SaveBossState()
    {
        PlayerPrefs.SetInt("BossActive", this.gameObject.activeSelf ? 1 : 0);
        PlayerPrefs.SetFloat("BossPosX", this.transform.position.x);
        PlayerPrefs.SetFloat("BossPosY", this.transform.position.y);
        PlayerPrefs.SetFloat("BossPosZ", this.transform.position.z);
        PlayerPrefs.SetInt("BossIsDead", isDead ? 1 : 0);
        PlayerPrefs.SetInt("BossHealth", health);
        PlayerPrefs.SetInt("QuestActivated", questActivated ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadBossState()
    {
        if (PlayerPrefs.HasKey("BossActive"))
        {
            this.gameObject.SetActive(PlayerPrefs.GetInt("BossActive") == 1);
            float x = PlayerPrefs.GetFloat("BossPosX");
            float y = PlayerPrefs.GetFloat("BossPosY");
            float z = PlayerPrefs.GetFloat("BossPosZ");
            this.transform.position = new Vector3(x, y, z);
            isDead = PlayerPrefs.GetInt("BossIsDead") == 1;
            health = PlayerPrefs.GetInt("BossHealth");
            questActivated = PlayerPrefs.GetInt("QuestActivated") == 1;
        }
    }
}
