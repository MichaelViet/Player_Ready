using System.Collections.Generic;
using UnityEngine;
public class WizardController : MonoBehaviour
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pressE.SetActive(false);
        dialogReader.bottomBarCanvasGroup.alpha = 0;
        dialogReader.OnDialogComplete += OnDialogComplete;
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
                if (dialogReader.bottomBarCanvasGroup.alpha == 0) pressE.SetActive(true); // Замініть на alpha
            }
            else
            {
                inInteractionDistance = false;
                pressE.SetActive(false);
                dialogReader.bottomBarCanvasGroup.alpha = 0; // Замініть на alpha
            }
            if (dialogComplete)
            {
                //MoveToTargetXPosition();
                FaceDirectionOfMovement();
            }
            else if (inInteractionDistance) // Дивимося на гравця, якщо на відстані взаємодії
            {
                FacePlayer(closestPlayer);
            }

            if (inInteractionDistance && Input.GetKeyDown(KeyCode.E))
            {
                pressE.SetActive(false);
                dialogReader.bottomBarCanvasGroup.alpha = 1;
            }

            if (dialogReader.bottomBarCanvasGroup.alpha == 1 && Input.GetKeyDown(KeyCode.X))
            {
                pressE.SetActive(true);
                dialogReader.bottomBarCanvasGroup.alpha = 0;
            }

            if (dialogComplete)
            {
                MoveToTargetXPosition();
            }
            if (transform.position.x <= -34)
            {
                interactionDistance = 0;
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
            dialogComplete = false;
            animator.SetFloat("Speed", 0f);
        }
    }

    public void OnDialogComplete()
    {
        dialogComplete = true;
        Wall.SetActive(false);
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