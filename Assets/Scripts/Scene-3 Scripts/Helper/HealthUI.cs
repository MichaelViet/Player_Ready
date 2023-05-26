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
    float interactionRadius = 5f;

    void Start()
    {
        cam = Camera.main.transform;

        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                ui = Instantiate(uiPrefab, c.transform).transform;
                healthSlider = ui.GetChild(0).GetComponent<Image>();
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
}
