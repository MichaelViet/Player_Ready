using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlots[] inventorySlots;
    public KeyCode pickupKey = KeyCode.E;
    public PowerStone powerStone;
    public QuestSystem questSystem;
    public GameObject arena;
    public GameObject Boss;
    public bool questActivated = false;
    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(pickupKey) && powerStone != null && powerStone.IsPlayerInRange(5f) && !IsPowerStoneInInventory()) // Додано перевірку нової змінної
        {
            AddPowerStoneToInventory();
        }
    }

    public bool IsPowerStoneInInventory()
    {
        foreach (InventorySlots slot in inventorySlots)
        {
            if (slot.GetItem() == powerStone)
            {
                return true;
            }
        }

        return false;
    }

    public void AddPowerStoneToInventory()
    {
        arena.SetActive(true);
        Boss.SetActive(true);

        foreach (InventorySlots slot in inventorySlots)
        {
            if (slot.IsEmpty())
            {
                slot.AddItem(powerStone);
                powerStone.gameObject.SetActive(false);
                if (powerStone.canvasE != null)
                {
                    powerStone.canvasE.SetActive(false); // Приховуємо canvasE, коли гравець підіймає камінь
                }
                powerStone.gameObject.SetActive(false);
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
                    questActivated = true;
                }
                break;
            }
        }
    }

    public void DropPowerStone()
    {
        foreach (InventorySlots slot in inventorySlots)
        {
            if (slot.GetItem() == powerStone)
            {
                slot.RemoveItem();
                powerStone.gameObject.SetActive(true);
                break;
            }
        }
    }
}
