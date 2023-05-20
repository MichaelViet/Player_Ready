using UnityEngine;

/*  Базовый класс, из которого игрок и враги могут получить статистику. */

public class CharacterStats : MonoBehaviour {

	// Здоровье
	public int maxHealth = 150;
	public int currentHealth { get; private set; }
	public float TimeDelay = 2;
	public float TimeDelayHp;
	
	public Stat damage;
	public Stat armor;

    public event System.Action<int, int> OnHealthChanged;

	// Установите текущее состояние здоровья на максимальное при запуске игры

	void Awake ()
	{
		currentHealth = maxHealth;
	}

	// Урон по персонажу
	public void TakeDamage (int damage)
	{
		// Вычитание брони
		damage -= armor.GetValue();
		damage = Mathf.Clamp(damage, 0, int.MaxValue);

		// Урон злому персонажу
		currentHealth -= damage;
		Debug.Log(transform.name + " takes " + damage + " damage.");

        if (OnHealthChanged != null)
        {
            OnHealthChanged(maxHealth, currentHealth);
        }

		// If health reaches zero
		if (currentHealth <= 0)
		{
			Die();
		}

    }

	public virtual void Die ()
	{
		// Die in some way
		// This method is meant to be overwritten
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
            }
        }
	}
}
