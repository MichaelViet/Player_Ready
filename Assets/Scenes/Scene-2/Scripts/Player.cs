using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    public Slider healthSlider;
    public int health;
    public CanvasGroup healthSliderCanvasGroup;
    private float lastDamageTime;
    public float healthRegenDelay = 5f;  // здоров'я відновлюється кожні 5 секунд
    public int healthRegenAmount = 2;  // кількість здоров'я, що відновлюється за один раз
    private CameraOffsetAnimator cameraOffsetAnimator;

    void Start()
    {
        cameraOffsetAnimator = FindObjectOfType<CameraOffsetAnimator>();
        CameraOffsetAnimator.OnAnimationEnd += EnableMovement;
        dialogReader = FindObjectOfType<DialogReader>();
        rb = GetComponent<Rigidbody2D>();
        healthSlider.maxValue = 250;
        healthSlider.value = health;
        lastDamageTime = Time.time;
        healthSliderCanvasGroup = healthSlider.GetComponent<CanvasGroup>();
        UpdateHealthSliderVisibility();
        StartCoroutine(RegenHealth());
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        healthSlider.value = health;
        lastDamageTime = Time.time;
        UpdateHealthSliderVisibility();
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
        healthSlider.value = health;
    }

    IEnumerator RegenHealth()
    {
        while (true)  // постійний цикл
        {
            if (Time.time - lastDamageTime >= healthRegenDelay && health < 250)
            {
                RegenerateHealth();
            }
            yield return new WaitForSeconds(healthRegenDelay);
        }
    }

    private void RegenerateHealth()
    {
        health += healthRegenAmount;
        healthSlider.value = health;
        UpdateHealthSliderVisibility();
    }

    private void UpdateHealthSliderVisibility()
    {
        if (health < 250)
        {
            healthSliderCanvasGroup.alpha = 1;
        }
        else
        {
            healthSliderCanvasGroup.alpha = 0;
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