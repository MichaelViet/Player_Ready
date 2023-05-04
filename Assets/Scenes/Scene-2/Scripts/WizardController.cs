using UnityEngine;

public class WizardController : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 3.0f;
    public Animator animator;
    public GameObject pressE;
    public CanvasGroup bottomBarCanvasGroup; // Змініть на CanvasGroup
    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private bool inInteractionDistance;
    public bool dialogComplete = false;
    private float moveSpeed = 3f;
    private float targetXPosition = -37;
    public DialogReader dialogReader;
    public GameObject Wall;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pressE.SetActive(false);
        bottomBarCanvasGroup.alpha = 0; // Замініть на alpha
        dialogReader.OnDialogComplete += OnDialogComplete;
    }
    private void OnDestroy()
    {
        dialogReader.OnDialogComplete -= OnDialogComplete; // Відписуємося від події, коли об'єкт знищено
    }
    void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= interactionDistance)
            {
                inInteractionDistance = true;
                if (bottomBarCanvasGroup.alpha == 0) pressE.SetActive(true); // Замініть на alpha
            }
            else
            {
                inInteractionDistance = false;
                pressE.SetActive(false);
                bottomBarCanvasGroup.alpha = 0; // Замініть на alpha
            }
            if (dialogComplete)
            {
                //MoveToTargetXPosition();
                FaceDirectionOfMovement();
            }
            else if (inInteractionDistance) // Дивимося на гравця, якщо на відстані взаємодії
            {
                FacePlayer();
            }

            if (inInteractionDistance && Input.GetKeyDown(KeyCode.E))
            {
                pressE.SetActive(false);
                bottomBarCanvasGroup.alpha = 1;
            }

            if (bottomBarCanvasGroup.alpha == 1 && Input.GetKeyDown(KeyCode.X))
            {
                pressE.SetActive(true);
                bottomBarCanvasGroup.alpha = 0;
            }

            if (dialogComplete)
            {
                MoveToTargetXPosition();
            }
        }
    }
    private void FacePlayer()
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
        animator.SetFloat("Speed", moveSpeed * (distanceToTarget > 0.1f ? 1f : 0f));

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
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}