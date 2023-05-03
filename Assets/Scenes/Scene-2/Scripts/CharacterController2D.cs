using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f; // Сила стрибка
    [Range(0, 1)][SerializeField] private float m_CrouchSpeed = .36f; // Швидкість зниження в режимі пригнушення
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f; // Плавність руху
    [SerializeField] private bool m_AirControl = false; // Чи може гравець керувати в повітрі
    [SerializeField] private LayerMask m_WhatIsGround; // Шар, який відповідає за ґрунт
    [SerializeField] private Transform m_GroundCheck; // Точка, що перевіряє чи є гравець на землі
    [SerializeField] private Transform m_CeilingCheck; // Точка, що перевіряє чи гравець находиться під стелею
    [SerializeField] private Collider2D m_CrouchDisableCollider; // Колайдер, який вимикається в режимі пригнушення

    const float k_GroundedRadius = .2f; // Радіус круга для перевірки знаходження на ґрунті
    private bool m_Grounded; // Чи знаходиться гравець на землі
    const float k_CeilingRadius = .2f; // Радіус круга для перевірки знаходження під стелею
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true; // Напрямок в який дивиться гравець
    private Vector3 m_Velocity = Vector3.zero; // Вектор швидкості гравця

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent; // Подія, яка відбувається коли гравець опускається на землю

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent; // Подія, яка відбувається коли гравець переходить в режим пригнушення
    private bool m_wasCrouching = false; // Чи перебуває гравець в режимі пригнушення

    // Встановлюємо m_Rigidbody2D за допомогою функції GetComponent
    // Якщо подія OnLandEvent не існує, то створюємо її
    // Якщо подія OnCrouchEvent не існує, то створюємо її
    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }


    // Функція, яка викликається з фіксованою частотою і відповідає за зміну положення об'єкту в гравітаційному полі
    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded; // Збереження попереднього стану змінної, що відповідає за знаходження об'єкту на землі
        // Об'єкт зараз не знаходиться на землі
        m_Grounded = false; // Задаємо силу гравітації, яка буде діяти на об'єкт
        float gravityForce = -9.81f; // Створюємо вектор, що вказує на напрямок дії сили гравітації
        Vector2 downForce = new Vector2(0, gravityForce); // Додаємо до об'єкту силу гравітації
        m_Rigidbody2D.AddForce(downForce); // Знаходимо всі колайдери, з якими перетинається колайдер, що перевіряє, чи об'єкт на землі
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);

        // Проходимося по кожному колайдеру, що перетинається з колайдером, що перевіряє, чи об'єкт на землі
        for (int i = 0; i < colliders.Length; i++)
        {
            // Якщо колайдер не належить поточному об'єкту, то об'єкт знаходиться на землі
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                // Якщо об'єкт тільки що залетів на землю, викликаємо подію OnLandEvent
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }


    // Функція, яка відповідає за рух головного героя
    public void Move(float move, bool crouch, bool jump)
    {
        // Якщо герой не кричиться, перевіряємо, чи може він присісти, щоб не вдаритись об стелю
        if (!crouch)
        {
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        // Якщо герой на землі або може керувати повітрям, виконуємо рух
        if (m_Grounded || m_AirControl)
        {
            // Якщо герой кричиться, то присідаємо і знижуємо швидкість руху
            if (crouch)
            {
                // Якщо герой тільки що почав кричатися, викликаємо подію OnCrouchEvent
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }
                move *= m_CrouchSpeed;

                // Вимикаємо колайдер, який заважає кричатися під об'єктами
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Увімкнути колайдер, який заважає кричатися під об'єктами
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                // Якщо герой тільки що перестав кричатися, викликаємо подію OnCrouchEvent
                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            // Задаємо цільову швидкість героя
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);

            // Застосовуємо згладжування руху
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // Якщо герой рухається вправо і не повернутий вправо, то повертаємо його вправо
            if (move > 0 && !m_FacingRight)
            {
                Flip();
            }

            else if (move < 0 && m_FacingRight)
            {
                // Якщо герой рухається вліво і не повернутий вліво, то повертаємо його вліво
                Flip();
            }
        }

        // Якщо герой на землі і натиснута клавіша прискоку, то герой відлітає вгору
        if (m_Grounded && jump)
        {
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    // Функція, яка повертає героя в протилежний бік
    private void Flip()
    {
        // Змінюємо напрямок, в який дивиться герой
        m_FacingRight = !m_FacingRight;

        // Обертаємо героя в протилежний бік
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
