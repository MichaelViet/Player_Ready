using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    #region Singleton

    public static Inventory instance; // Екземпляр сінглтона
    void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this; // Встановлюємо поточний екземпляр як єдиний екземпляр сінглтона
    }
    #endregion

    public delegate void OnItemChanged(); // Делегат, що викликається при зміні інвентаря
    public OnItemChanged onItemChangedCallback; // Колбек, який буде викликаний при зміні інвентаря
    private PlayerController player; // Посилання на гравця
    public int space = 20; // Максимальна кількість предметів, які можуть бути в інвентарі
    public List<Item> items = new List<Item>(); // Список предметів в інвентарі

    public void Start()
    {
        player = FindObjectOfType<PlayerController>(); // Знаходимо компонент PlayerController
    }

    public bool Add(Item item)
    {
        if (!item.isDefaultItem) // Перевіряємо, чи предмет не є типовим предметом
        {
            if (items.Count >= space) // Перевіряємо, чи немає місця в інвентарі
            {
                Debug.Log("Not enough room."); // Виводимо повідомлення про недостатньо місця в інвентарі
                return false; // Повертаємо false, щоб показати, що предмет не був доданий
            }

            items.Add(item); // Додаємо предмет до списку інвентаря

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke(); // Викликаємо колбек, що сигналізує про зміну інвентаря
        }

        return true; // Повертаємо true, щоб показати, що предмет був успішно доданий
    }

    public void Remove(Item item)
    {
        // Якщо предмет не є предметом для завдання, видаляємо його
        if (!item.isMissionItem)
        {
            items.Remove(item); // Видаляємо предмет з інвентаря

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke(); // Викликаємо колбек, що сигналізує про зміну інвентаря
        }
    }
    public List<Equipment> allEquipments;

    public Equipment GetEquipmentById(string id)
    {
        foreach (var equipment in allEquipments)
        {
            if (equipment.itemId == id)
            {
                return equipment;
            }
        }
        return null;
    }

}
