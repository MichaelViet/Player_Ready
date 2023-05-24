using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;
    public GameObject itemPrefab;
    public bool isPickedUp = false;
    public string itemId = Guid.NewGuid().ToString();
    public bool isMissionItem = false;
    public virtual void Use()
    {
        Debug.Log("Using " + name);
    }

    public void RemoveFromInventory()
    {
        // Якщо предмет не є предметом для завдання, видаляємо його
        if (!isMissionItem)
        {
            isPickedUp = false;
            Inventory.instance.Remove(this);
        }
        else
        {
            Debug.Log(name + " is a mission item and cannot be removed."); // Виводимо повідомлення, що предмет є предметом для завдання і не може бути видалений
        }
    }

}
