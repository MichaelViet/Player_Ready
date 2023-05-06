using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Клас, який відповідає за рух гравця
public class PlayerMovement : MonoBehaviour
{
    // Посилання на CharacterController2D компонент
    [SerializeField] private CharacterController2D controller;
    // Посилання на Animator компонент
    [SerializeField] private Animator animator;
    // Швидкість бігу гравця
    [SerializeField] private float runSpeed = 40f;
    // Змінні, які використовуються для зберігання даних про рух гравця
    private DialogReader dialogReader;
    private bool canMove = true;
    private float horizontalMove = 0f;
    private bool jump = false;
    private bool crouch = false;

    void Start()
    {
        dialogReader = FindObjectOfType<DialogReader>();
    }

    // Функція, яка викликається при кожному кадрі
    private void Update()
    {
        // Якщо гра зупинена, то не продовжуємо виконання коду
        if (PauseMenu.isPaused) return;

        // Отримуємо значення горизонтального входу користувача
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        // Встановлюємо значення швидкості для анімації бігу
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // Якщо користувач натиснув кнопку пробіл, то позначаємо, що гравець хоче зробити прижок
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        // Якщо користувач натиснув кнопку присідання, то позначаємо, що гравець хоче присісти
        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        // Якщо користувач відпустив кнопку присідання, то позначаємо, що гравець більше не хоче присідати
        else if (Input.GetButtonUp("Crouch"))
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
}