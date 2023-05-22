using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterStats))]
public class HealthUI : MonoBehaviour
{
    public GameObject uiPrefab;
    public Transform player;
    public Transform target;
    float visibleTime = 5;
    float lastMadeVisibleTime;
    Transform ui;
    Image healthSlider;
    Transform cam;
    CanvasGroup canvasGroup;

    float interactionRadius = 5f; // radius for interaction

    void Start()
    {
        cam = Camera.main.transform;

        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                ui = Instantiate(uiPrefab, c.transform).transform;
                healthSlider = ui.GetChild(0).GetComponent<Image>();
                canvasGroup = ui.GetComponent<CanvasGroup>(); // get the CanvasGroup component
                ui.gameObject.SetActive(false);
                break;
            }
        }

        GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;
    }

    void OnHealthChanged(int maxHealth, int currentHealth)
    {
        if (ui != null)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            healthSlider.fillAmount = healthPercent;
            if (currentHealth <= 0)
            {
                Destroy(ui.gameObject);
            }
        }
    }

    void LateUpdate()
    {
        if (ui != null)
        {
            ui.position = target.position;
            ui.forward = -cam.forward;

            // Check the distance to the player
            if (Vector3.Distance(player.position, target.position) <= interactionRadius)
            {
                ui.gameObject.SetActive(true);
                lastMadeVisibleTime = Time.time;
            }
            else if (Time.time - lastMadeVisibleTime > visibleTime)
            {
                ui.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if "F" is pressed
        if (Input.GetKeyDown(KeyCode.F) && canvasGroup != null)
        {
            // set CanvasGroup's alpha to 0
            canvasGroup.alpha = 0;
        }
    }
}
