using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public int maxHealth = 150;
    private int _currentHealth; // Додайте приватне поле для збереження значення currentHealth

    public int currentHealth
    {
        get { return _currentHealth; }
        private set { _currentHealth = value; }
    }

    public float TimeDelay = 2;
    public float TimeDelayHp;

    public Stat damage;
    public Stat armor;

    public event System.Action<int, int> OnHealthChanged;

    public void Awake()
    {
        currentHealth = maxHealth;
    }
    public void SetCurrentHealth(int health)
    {
        _currentHealth = health; // Встановіть значення в приватне поле _currentHealth
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
