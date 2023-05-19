using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ChooseLabelController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Color defaultColor; // Колір тексту за умовчанням
    public Color hoverColor; // Колір тексту при наведенні на нього миші
    private StoryScene scene; // Посилання на скріпт StoryScene
    private TextMeshProUGUI textMesh; // Посилання на компонент TextMeshProUGUI цього об'єкта
    private ChooseController controller; // Посилання на скріпт ChooseController

    ChooseScene.ChooseLabel currentLabel;

    // Awake функція викликається, при створенні екзепляра об'єкта
    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>(); // Отримаємо компонент TextMeshProUGUI цього об'єкта
        textMesh.color = defaultColor; // Встановлюємо колір тексту за замовчуванням
    }

    // Функція GetHeight повертає висоту тексту в компоненті TextMeshProUGUI
    public float GetHeight()
    {
        return textMesh.rectTransform.sizeDelta.y * textMesh.rectTransform.localScale.y;
    }
    //Функція Setup використовується для налаштування об’єкта з заданою інформацією про мітку, 
    //посиланням на контролер і положенням "y"
    public void Setup(ChooseScene.ChooseLabel label, ChooseController controller, float y)
    {
        currentLabel = label; // Зберігаємо структуру ChooseLabel
        scene = label.nextScene; // Зберігаємо посилання на наступну сцену
        textMesh.text = label.text; // Встановити текст компонента TextMeshProUGUI в текст мітки
        this.controller = controller; // Зберігаємо посилання на скріпт ChooseController

        Vector3 position = textMesh.rectTransform.localPosition;
        position.y = y;
        textMesh.rectTransform.localPosition = position;
    }

    // Ця функція викликається, при натисканні на об'єкт
    public void OnPointerClick(PointerEventData eventData)
    {
        // Викликаємо функцію PerformChoose скріпта ChooseController, передаючи структуру ChooseLabel
        controller.PerformChoose(currentLabel);
    }
    // Ця функція викликається, коли вказівник миші наводиться на об'єкт
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Встановлюємо колір тексту при наведенні
        textMesh.color = hoverColor;
    }

    // Ця функція викликається, коли покажчик миші виходить з об'єкта
    public void OnPointerExit(PointerEventData eventData)
    {
        // Встановлюємо колір тексту за замовчуванням
        textMesh.color = defaultColor;
    }
}
