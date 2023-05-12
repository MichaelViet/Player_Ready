using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public QuestSystem questSystem;
    public Quest questToStart;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            questSystem.StartQuest(questToStart);
            gameObject.SetActive(false);
        }
    }
}
