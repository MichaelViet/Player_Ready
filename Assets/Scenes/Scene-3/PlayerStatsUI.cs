using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Потрібно для роботи з текстовими полями TMP

public class PlayerStatsUI : MonoBehaviour
{
    public PlayerStats playerStats; // Посилання на об'єкт PlayerStats

    // Посилання на об'єкти TMP
    public TextMeshProUGUI HealthTMP;
    public TextMeshProUGUI ArmorTMP;
    public TextMeshProUGUI DamageTMP;

    private void Start()
    {
        // Підписатися на подію зміни здоров'я
        playerStats.OnHealthChanged += UpdateHealthUI;

        // Початкове оновлення UI
        UpdateHealthUI(playerStats.maxHealth, playerStats.currentHealth);
        UpdateArmorUI();
        UpdateDamageUI();
    }

    // Функція для оновлення UI здоров'я
    private void UpdateHealthUI(int maxHealth, int currentHealth)
    {
        HealthTMP.text = currentHealth.ToString();
    }

    // Функції для оновлення UI броні і пошкоджень
    private void UpdateArmorUI()
    {
        ArmorTMP.text = playerStats.armor.GetValue().ToString();
    }

    private void UpdateDamageUI()
    {
        DamageTMP.text = playerStats.damage.GetValue().ToString();
    }

    private void Update()
    {
        // Перевірка, чи оновлювати UI броні або пошкоджень
        if (ArmorTMP.text != playerStats.armor.GetValue().ToString())
            UpdateArmorUI();

        if (DamageTMP.text != playerStats.damage.GetValue().ToString())
            UpdateDamageUI();
    }
}
