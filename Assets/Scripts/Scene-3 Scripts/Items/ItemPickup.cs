using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public HintManager hintManager;
    private bool hintShown = false; // Відслідковує, чи був показаний підказка
    private void Start()
    {
        hintManager = FindObjectOfType<HintManager>();
    }
    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    void PickUp()
    {
        Debug.Log("Picking up " + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);
        hintManager.ShowHint(2); // Викликаємо ShowHint з індексом 1
        hintShown = true; // Оновлюємо, що підказка була показана
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
    }
}
