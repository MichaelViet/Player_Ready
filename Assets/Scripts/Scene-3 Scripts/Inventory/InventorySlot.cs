using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public GameObject itemPrefab; // Префаб предмета для створення
    public Transform player; // Посилання на гравця

    Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        // Створюємо новий предмет в місці гравця
        GameObject droppedItem = Instantiate(item.itemPrefab, player.position, Quaternion.identity);

        // Видаляємо предмет з інвентаря
        Inventory.instance.Remove(item);
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }
}
