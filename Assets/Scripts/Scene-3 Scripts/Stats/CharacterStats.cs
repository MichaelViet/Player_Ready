using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public int maxHealth = 150;
    public int currentHealth { get; private set; }
    public float TimeDelay = 2;
    public float TimeDelayHp;

    public Stat damage;
    public Stat armor;

    public event System.Action<int, int> OnHealthChanged;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (OnHealthChanged != null)
        {
            OnHealthChanged(maxHealth, currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    public virtual void Die()
    {
        Debug.Log(transform.name + " died.");
    }

    void Update()
    {
        if (currentHealth < maxHealth)
        {
            TimeDelayHp += Time.deltaTime;
            if (TimeDelayHp >= TimeDelay)
            {
                currentHealth++;
                TimeDelayHp = 0;
                if (currentHealth >= maxHealth)
                {
                    currentHealth = maxHealth;
                }

                // викликати подію OnHealthChanged
                if (OnHealthChanged != null)
                {
                    OnHealthChanged(maxHealth, currentHealth);
                }
            }
        }
    }
}
