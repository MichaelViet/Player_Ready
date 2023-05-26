using UnityEngine;

public class GenericUI : MonoBehaviour
{
    public GameObject uiPrefab;
    public Transform player;
    public Transform target;
    float visibleTime = 5;
    float lastMadeVisibleTime;
    Transform ui;
    Transform cam;
    CanvasGroup canvasGroup;
    float interactionRadius = 5f;

    void Start()
    {
        cam = Camera.main.transform;

        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                ui = Instantiate(uiPrefab, c.transform).transform;
                canvasGroup = ui.GetComponent<CanvasGroup>();
                ui.gameObject.SetActive(false);
                break;
            }
        }
    }

    void LateUpdate()
    {
        if (ui != null)
        {
            ui.position = target.position;
            ui.forward = -cam.forward;

            float distanceToPlayer = Vector3.Distance(player.position, target.position);
            if (distanceToPlayer <= interactionRadius)
            {
                ui.gameObject.SetActive(true);
                lastMadeVisibleTime = Time.time;
            }
            else if (Time.time - lastMadeVisibleTime > visibleTime || distanceToPlayer > interactionRadius)
            {
                ui.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, target.position);
        if (Input.GetKeyDown(KeyCode.F) && canvasGroup != null && ui.gameObject.activeInHierarchy && distanceToPlayer <= interactionRadius)
        {
            canvasGroup.alpha = 0;
        }
    }

}
