using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public Transform playerTransform;
    private PowerStone item;

    private void Start()
    {
        removeButton.onClick.AddListener(RemoveItem);
        removeButton.gameObject.SetActive(false);
    }

    public void AddItem(PowerStone newItem)
    {
        item = newItem;
        icon.sprite = newItem.GetComponent<SpriteRenderer>().sprite;
        icon.enabled = true;
        removeButton.gameObject.SetActive(true);
    }

    public void RemoveItem()
    {
        if (item != null)
        {
            Vector3 playerPosition = playerTransform.position;
            Vector3 itemPosition = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
            item.transform.position = itemPosition + playerTransform.right;

            item.SetInitialPosition(item.transform.position);  // <-- Встановлюємо нову початкову позицію

            item.gameObject.SetActive(true);
            item = null;

            icon.sprite = null;
            icon.enabled = false;
            removeButton.gameObject.SetActive(false);
        }
    }
    public PowerStone GetItem()
    {
        return item;
    }
    public bool IsEmpty()
    {
        return item == null;
    }
}
