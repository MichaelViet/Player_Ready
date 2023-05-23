using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public HintManager hintManager;
    private bool hintShown = false; // Відслідковує, чи був показаний підказка

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    private void Start()
    {
        hintManager = FindObjectOfType<HintManager>();

        // Перевіряємо, чи існує екземпляр LevelThreeController та чи є дані про стан предметів
        if (!ReferenceEquals(LevelThreeController.instance, null) && !ReferenceEquals(LevelThreeController.instance.data, null) && LevelThreeController.instance.data.itemStates != null)
        {
            // Знаходимо стан поточного предмета в збережених даних
            var itemState = LevelThreeController.instance.data.itemStates.Find(itemState => itemState.itemId == item.itemId);
            if (itemState != null && itemState.isPickedUp)
            {
                // Якщо предмет вже піднятий, знищуємо його
                Destroy(gameObject);
            }
        }
    }

    void PickUp()
    {
        Debug.Log("Підняття " + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);

        // Показуємо підказку з індексом 2
        hintManager.ShowHint(2);
        hintShown = true;

        if (wasPickedUp)
        {
            item.isPickedUp = true; // Встановлюємо прапорець, що предмет піднятий
            Destroy(gameObject);
        }
    }

}
