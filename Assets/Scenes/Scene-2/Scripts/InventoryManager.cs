using System.Collections;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public KeyCode pickupKey = KeyCode.E;
    public PowerStone powerStone;
    private QuestSystem questSystem;
    private bool questActivated;
    public GameObject arena;
    public GameObject Boss;
    private void Start()
    {
        questSystem = FindObjectOfType<QuestSystem>();
        questActivated = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(pickupKey) && powerStone != null && powerStone.IsPlayerInRange(3f) && !IsPowerStoneInInventory())
        {
            AddPowerStoneToInventory();
        }
    }

    private bool IsPowerStoneInInventory()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.GetItem() == powerStone)
            {
                return true;
            }
        }
        return false;
    }

    private void AddPowerStoneToInventory()
    {
        arena.SetActive(true);
        Boss.SetActive(true);
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.IsEmpty())
            {
                slot.AddItem(powerStone);
                powerStone.gameObject.SetActive(false);
                break;
            }
        }
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
    }

}

