using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    public float interactionRadius = 3f;
    public DialogReader dialogReader;
    public TextAsset dialogJson;
    public bool isPlayerInRange = false;
    public bool hasDialogueFinished = false;
    public LayerMask playerLayer;

    private void Update()
    {
        isPlayerInRange = Physics.CheckSphere(transform.position, interactionRadius, playerLayer);
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && !hasDialogueFinished)
        {
            dialogReader.LoadDialog(dialogJson);
            dialogReader.OnDialogComplete += HandleDialogComplete;
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
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
