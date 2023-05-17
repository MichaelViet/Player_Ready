using System.Collections.Generic;
using UnityEngine;
public class WizzardController : MonoBehaviour
{
    public List<Transform> players;
    public float interactionDistance = 3.0f;
    public Animator animator;
    public GameObject pressE;
    public DialogReader dialogReader;
    public GameObject Wall;
    public bool dialogComplete = false;
    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private bool inInteractionDistance;
    private float moveSpeed = 3f;
    private float smoothTime = 0.1f;
    private float targetXPosition = -37;
    private float currentSpeed;
    private bool hintShown = false;
    public RayCastWeapon playerWeapon;
    private QuestSystem questSystem;
    private bool questActivated;
    public bool bossIsDead = false; // Додаємо новий параметр для відстеження стану босса
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pressE.SetActive(false);
        dialogReader.bottomBarCanvasGroup.alpha = 0;
        dialogReader.OnDialogComplete += OnDialogComplete;
        questSystem = FindObjectOfType<QuestSystem>();
        questActivated = false;
    }

    void Update()
    {
        if (players != null && players.Count > 0)
        {
            Transform closestPlayer = GetClosestPlayer();
            float distance = Vector2.Distance(transform.position, closestPlayer.position);
            if (distance <= interactionDistance)
            {
                inInteractionDistance = true;
                if (dialogReader.bottomBarCanvasGroup.alpha == 0) pressE.SetActive(true);
            }
            else
            {
                inInteractionDistance = false;
                pressE.SetActive(false);
                dialogReader.bottomBarCanvasGroup.alpha = 0;
            }
            if (inInteractionDistance && Input.GetKeyDown(KeyCode.E))
            {
                pressE.SetActive(false);
                dialogReader.bottomBarCanvasGroup.alpha = 1;
                SetCanShoot(false);

                if (bossIsDead)
                {
                    dialogReader.LoadDialog(dialogReader.dialogJsonAfterBossDies); // Завантажуємо новий діалог, якщо босс вбитий
                }
            }
            if (dialogComplete)
            {
                MoveToTargetXPosition();
                FaceDirectionOfMovement();
                interactionDistance = 0;
                // Показати підказку після FadeIn, але лише один раз
                if (!hintShown)
                {
                    HintManager hintManager = FindObjectOfType<HintManager>();
                    if (hintManager != null)
                    {
                        hintManager.ShowHint(1);
                    }
                    hintShown = true;
                }
            }
            else if (inInteractionDistance) // Дивимося на гравця, якщо на відстані взаємодії
            {
                FacePlayer(closestPlayer);
            }
            if (dialogReader.bottomBarCanvasGroup.alpha == 1 && Input.GetKeyDown(KeyCode.X))
            {
                pressE.SetActive(true);
                dialogReader.bottomBarCanvasGroup.alpha = 0;
                SetCanShoot(true);
            }
        }
    }

    private Transform GetClosestPlayer()
    {
        Transform closestPlayer = null;
        float minDistance = float.MaxValue;

        foreach (Transform player in players)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    private void FacePlayer(Transform player)
    {
        if (inInteractionDistance) // Додаємо перевірку на відстань
        {
            Vector2 direction = (player.position - transform.position).normalized;
            if (direction.x > 0f)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (direction.x < 0f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    private void FaceDirectionOfMovement()
    {
        float xDirection = transform.position.x - targetXPosition;

        if (Mathf.Abs(xDirection) > 0.1f)
        {
            if (xDirection > 0f)
            {
                transform.localScale = new Vector3(-1, 1, 1); // НПС повернутий вліво
            }
            else if (xDirection < 0f)
            {
                transform.localScale = new Vector3(1, 1, 1); // НПС повернутий вправо
            }
        }
    }

    private void MoveToTargetXPosition()
    {
        float step = moveSpeed * Time.deltaTime;
        Vector2 targetPosition = new Vector2(targetXPosition, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);

        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        float targetSpeed = moveSpeed * (distanceToTarget > 0.1f ? 1f : 0f);
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref currentVelocity.y, smoothTime);
        animator.SetFloat("Speed", currentSpeed);

        if (distanceToTarget <= 0.1f)
        {
            animator.SetFloat("Speed", 0f);
        }
    }
    private void SetCanShoot(bool value)
    {
        if (playerWeapon != null)
        {
            playerWeapon.SetCanShoot(value);
        }
    }
    public void OnDialogComplete()
    {
        dialogComplete = true;
        Wall.SetActive(false);
        SetCanShoot(true); // дозволити стрільбу після завершення діалогу

        if (bossIsDead) // Якщо босс мертвий, то це другий діалог
        {
            dialogComplete = false; // Встановлюємо dialog2Complete в true

        }
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

        StartCoroutine(questSystem.FadeIn());

    }

    private void OnDestroy()
    {
        dialogReader.OnDialogComplete -= OnDialogComplete; // Відписуємося від події, коли об'єкт знищено
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }

}