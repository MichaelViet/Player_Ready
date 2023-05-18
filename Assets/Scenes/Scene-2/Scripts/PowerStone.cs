using UnityEngine;

public class PowerStone : MonoBehaviour
{
    public GameObject player;
    public float speed = 1f;
    public float pickupRadius = 3f;
    public delegate void PowerStoneTakenHandler();
    public string id;
    private Rigidbody2D rb;
    private bool isFloating = true;
    private Vector3 initialPosition;
    private float time;
    public GameObject canvasE;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        time = 0f;
        if (canvasE != null)
        {
            canvasE.SetActive(false); // Приховуємо на початку
        }
    }

    public bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(player.transform.position, transform.position) <= range;
    }

    private void Update()
    {
        if (isFloating)
        {
            //  Функція для створення плавної анімації "плавання"
            float newY = initialPosition.y + Mathf.Sin(time * speed) * 2;
            transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
            time += Time.deltaTime;
        }
        if (canvasE != null)
        {
            canvasE.SetActive(IsPlayerInRange(pickupRadius)); // Відображаємо canvasE, якщо гравець в радіусі
        }
    }
    public void SetInitialPosition(Vector3 newPosition)
    {
        initialPosition = newPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
