using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Посилання на гравця
    public Vector3 offset; // Відстань камери від гравця
    public float zoomSpeed = 4f; // Швидкість зумування камери
    public float minZoom = 10f; // Мінімальний рівень зуму камери
    public float maxZoom = 5f; // Максимальний рівень зуму камери
    public float pitch = 2f; // Кут нахилу камери
    public float yawSpeed = 100f; // Швидкість обертання камери по горизонталі
    private float currentZoom = 5f; // Поточний рівень зуму камери
    private float currentYaw = 0f; // Поточний горизонтальний поворот камери

    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed; // Отримати значення прокрутки колеса миші для зумування камери
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom); // Обмежити поточний рівень зуму камери в межах мінімального і максимального значень

        if (Input.GetKey(KeyCode.Q))
        {
            currentYaw += yawSpeed * Time.deltaTime; // Збільшити горизонтальний поворот камери вліво
        }
        else if (Input.GetKey(KeyCode.E))
        {
            currentYaw -= yawSpeed * Time.deltaTime; // Збільшити горизонтальний поворот камери вправо
        }
    }

    void LateUpdate()
    {
        transform.position = player.position - offset * currentZoom; // Встановити позицію камери з урахуванням зуму та відстані від гравця
        transform.LookAt(player.position + Vector3.up * pitch); // Навести камеру на гравця з врахуванням нахилу
        transform.RotateAround(player.position, Vector3.up, currentYaw); // Обернути камеру навколо гравця по горизонталі
    }
}
