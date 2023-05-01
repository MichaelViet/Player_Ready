using UnityEngine;

public class WizardController : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 3.0f;
    public Animator animator;
    public GameObject pressE;
    public GameObject bottomBar;
    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private bool inInteractionDistance;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pressE.SetActive(false);
        bottomBar.SetActive(false);
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= interactionDistance)
            {
                inInteractionDistance = true;
                if (!bottomBar.activeSelf) pressE.SetActive(true);
            }
            else
            {
                inInteractionDistance = false;
                pressE.SetActive(false);
                bottomBar.SetActive(false);
            }

            if (inInteractionDistance && Input.GetKeyDown(KeyCode.E))
            {
                pressE.SetActive(false);
                bottomBar.SetActive(true);
            }

            if (bottomBar.activeSelf && Input.GetKeyDown(KeyCode.X))
            {
                pressE.SetActive(true);
                bottomBar.SetActive(false);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}