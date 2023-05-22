using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    public float interactionRadius = 3f;
    public DialogReader dialogReader;
    public TextAsset dialogJson;
    private bool isPlayerInRange = false;

    private void Start()
    {
        dialogReader.OnDialogComplete += HandleDialogComplete;
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            dialogReader.LoadDialog(dialogJson);
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
        interactionRadius = 0;
    }
}
