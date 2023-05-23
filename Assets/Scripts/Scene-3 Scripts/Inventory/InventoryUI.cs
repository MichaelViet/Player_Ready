using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    private HintManager hintManager;
    private bool hintShown = false; // Відслідковує, чи був показаний підказка
    private Inventory inventory;
    private InventorySlot[] slots;
    private bool inventoryIsOpen = false; // Відслідковує, чи відкритий інвентар

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
        hintManager = FindObjectOfType<HintManager>();
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);

            // Оновлюємо стан інвентаря
            inventoryIsOpen = inventoryUI.activeSelf;

            // Якщо інвентар було закрито, показуємо підказку з індексом 4
            if (!inventoryIsOpen && !hintShown)
            {
                hintManager.ShowHint(4);
                hintShown = true; // Оновлюємо, що підказка була показана
            }
            // Якщо інвентар було відкрито, показуємо підказку з індексом 3
            else if (inventoryIsOpen)
            {
                hintManager.ShowHint(3);
            }
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
