using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    public Interactable focus;
    public LayerMask movementMask;
    Camera cam;
    PlayerMotor motor;

    void Start()
    {
        // Отримуємо посилання на основну камеру та компонент PlayerMotor
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        // Перевіряємо, чи наведений курсор на UI-елемент
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // Обробка правої кнопки миші (рух до точки або взаємодія з об'єктом)
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Перевіряємо, чи попадає промінь на об'єкт, який має шар movementMask
            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                motor.MoveToPoint(hit.point);
                RemoveFocus();
            }
        }

        // Обробка правої кнопки миші (встановлення фокусу на взаємодію)
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Перевіряємо, чи попадає промінь на об'єкт
            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable);
                }
            }
        }
    }

    // Встановлення фокусу на взаємодію з об'єктом
    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
                focus.OnDefocused();

            focus = newFocus;
            motor.FollowTarget(newFocus);
        }
        newFocus.OnFocused(transform);
    }

    // Зняття фокусу з об'єкта
    void RemoveFocus()
    {
        if (focus != null)
            focus.OnDefocused();

        focus = null;
        motor.StopFollowingTarget();
    }
}
