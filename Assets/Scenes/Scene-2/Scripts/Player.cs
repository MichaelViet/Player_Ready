using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private Animator animator;
    [SerializeField] private float runSpeed = 40f;
    private DialogReader dialogReader;
    private float horizontalMove = 0f;
    private bool jump = false;
    private bool crouch = false;
    public bool canMove = false;
    private Rigidbody2D rb;
    public bool isSoldier = false;
    public int health;
    private CameraOffsetAnimator cameraOffsetAnimator;
    void Start()
    {
        cameraOffsetAnimator = FindObjectOfType<CameraOffsetAnimator>();
        CameraOffsetAnimator.OnAnimationEnd += EnableMovement;
        dialogReader = FindObjectOfType<DialogReader>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Якщо гра зупинена, то не продовжуємо виконання коду
        if (PauseMenu.isPaused)
        {
            return;
        }

        // Перевіртка playerStop в поточному MonologueZone і блокування хотьби, якщо це потрібно
        if (MonologueZone.currentZone != null)
        {
            canMove = !MonologueZone.currentZone.playerStop;
        }
        else
        {
            canMove = true;
        }
        // Отримуємо значення горизонтального входу користувача, якщо можна рухатися
        horizontalMove = canMove && !cameraOffsetAnimator.isCameraAnimating ? Input.GetAxisRaw("Horizontal") * runSpeed : 0;
        // Встановлюємо значення швидкості для анімації бігу
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        // Додаємо код для повороту персонажа в напрямку миші, якщо це солдат
        if (isSoldier)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePosition.x < transform.position.x)
            {
                controller.FlipToDirection(false);
            }
            else
            {
                controller.FlipToDirection(true);
            }
        }
        // Якщо можна рухатися, камера не анімується, і користувач натиснув кнопку пробіл, то позначаємо, що гравець хоче зробити прижок
        if (canMove && !cameraOffsetAnimator.isCameraAnimating && Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        // Якщо можна рухатися, камера не анімується, і користувач натиснув кнопку присідання, то позначаємо, що гравець хоче присісти
        if (canMove && !cameraOffsetAnimator.isCameraAnimating && Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        // Якщо можна рухатися, камера не анімується, і користувач відпустив кнопку присідання, то позначаємо, що гравець більше не хоче присідати
        else if (canMove && !cameraOffsetAnimator.isCameraAnimating && Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    // Функція, яка викликається після завершення прижку гравця
    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    // Функція, яка викликається при зміні стану присідання гравця
    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    // Функція, яка викликається кожен фіксований кадр
    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
    void OnDestroy()
    {
        CameraOffsetAnimator.OnAnimationEnd -= EnableMovement;  // відписатися від події
    }

    void EnableMovement()
    {
        canMove = true;
    }
}