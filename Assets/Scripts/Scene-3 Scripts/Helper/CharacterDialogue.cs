using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    public float interactionRadius = 3f;
    public bool isBoss = false;
    public GameObject BossSkeleton;
    public DialogReader dialogReader;
    public TextAsset dialogJson;
    public bool isPlayerInRange = false;
    public bool hasDialogueFinished = false;
    public LayerMask playerLayer;
    private HintManager hintManager;
    public QuestSystem questSystem;
    private bool questActivated = false;
    private bool hintShown = false; // Відслідковує, чи був показаний підказка
    public AudioClip nextMusic;
    public bool hasMusicChanged = false;
    private LevelThreeController levelThreeController;
    private void Start()
    {
        hintManager = FindObjectOfType<HintManager>();
        questSystem = FindObjectOfType<QuestSystem>();
        levelThreeController = FindObjectOfType<LevelThreeController>();
        if (isBoss)
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name == "BossSkeleton")
                {
                    BossSkeleton = obj;
                    break;
                }
            }
        }
    }
    private void Update()
    {
        isPlayerInRange = Physics.CheckSphere(transform.position, interactionRadius, playerLayer);
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && !hasDialogueFinished)
        {
            dialogReader.LoadDialog(dialogJson);
            dialogReader.OnDialogComplete += HandleDialogComplete;

        }
        if (hasDialogueFinished == true)
        {
            hintManager.ShowHint(1); // Викликаємо ShowHint з індексом 1
            hintShown = true; // Оновлюємо, що підказка була показана
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogReader.bottomBarCanvasGroup.alpha = 0;
        }
    }

    private void HandleDialogComplete()
    {
        hasDialogueFinished = true;
        dialogReader.OnDialogComplete -= HandleDialogComplete; // видаляємо обробник подій
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

        if (!hasMusicChanged)
        {
            // ваш код
            levelThreeController.levelMusic = nextMusic; // замінюємо музику в LevelManager
            levelThreeController.audioController.PlayAudio(levelThreeController.levelMusic, null, null); // відтворюємо нову музику
            hasMusicChanged = true;
        }
        if (isBoss)
        {
            BossSkeleton.SetActive(true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
