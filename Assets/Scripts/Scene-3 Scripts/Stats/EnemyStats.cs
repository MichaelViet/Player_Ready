using UnityEngine;

public class EnemyStats : CharacterStats
{
    public bool isBoss = false;
    public GameObject[] littleSkeletons;
    public bool isDead = false;
    public string skeletonID;
    private bool isTurnedOff = false;
    private QuestSystem questSystem;
    private bool questActivated = false;
    private bool hasMusicChanged = false;
    public AudioClip nextMusic;
    private LevelThreeController levelThreeController;
    public GameObject BossMusic;
    private void Start()
    {
        questSystem = FindObjectOfType<QuestSystem>();
        levelThreeController = FindObjectOfType<LevelThreeController>();
    }

    public override void Die()
    {
        base.Die();

        isDead = true; // Встановити цю змінну в true, коли бос мертвий

        if (isBoss)
        {
            // Активувати об'єкти LittleSkeleton
            foreach (GameObject skeleton in littleSkeletons)
            {
                skeleton.SetActive(true);
            }

            // Виконати код для активації квесту при вбивстві боса
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
            levelThreeController.levelMusic = nextMusic; // замінюємо музику в LevelManager
            levelThreeController.audioController.PlayAudio(levelThreeController.levelMusic, null, null); // відтворюємо нову музику
            hasMusicChanged = true;
        }
        BossMusic.SetActive(false);

        TurnOff();
    }
    private void TurnOff()
    {
        if (!isTurnedOff)
        {
            // Вимкнути об'єкт
            gameObject.SetActive(false);
            isTurnedOff = true;
        }
    }
}
