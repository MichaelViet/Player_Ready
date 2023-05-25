using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public HintManager hintManager;
    private bool hintShown = false; // Відслідковує, чи був показаний підказка
    private QuestSystem questSystem; // Посилання на систему квестів
    // Додаткові змінні для підрахунку піднятих айтемів
    public static int itemsPickedUpCount = 0;
    private static bool questActivated = false;
    private GameObject BossMusic;
    private void Start()
    {
        hintManager = FindObjectOfType<HintManager>();
        questSystem = FindObjectOfType<QuestSystem>();
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "BossMusic")
            {
                BossMusic = obj;
                break;
            }
        }
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

    public override void Interact()
    {
        base.Interact();

        PickUp();

        // Перевіряємо, чи було піднято достатню кількість айтемів для активації квесту
        if (itemsPickedUpCount >= 5 && !questActivated)
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
            BossMusic.SetActive(true);
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

            // Збільшуємо лічильник піднятих айтемів
            itemsPickedUpCount++;
            Debug.Log("Поточний itemsPickedUpCount: " + itemsPickedUpCount);
        }
    }

}
