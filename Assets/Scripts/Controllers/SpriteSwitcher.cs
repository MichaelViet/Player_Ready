using UnityEngine;
using UnityEngine.UI;
public class SpriteSwitcher : MonoBehaviour
{

    public bool isSwitched = false; // Ця логічна змінна використовується для відстеження того, яке з двох зображень зараз відображається
    public Image image1, image2; // Ці два компоненти зображення, які використовуватимуться для відображення спрайту

    private Animator animator; // Це компонент Animator, який використовуватиметься для анімації перемикання двох зображень

    private void Awake()
    {
        // Отримуємо компонент Animator з GameObject, до якого приєднано сценарій
        animator = GetComponent<Animator>();
    }

    // Метод SwitchImage використовується для перемикання відображеного спрайту між двома компонентами зображення
    public void SwitchImage(Sprite sprite)
    {
        // Якщо зараз відображається перше зображення, установіть спрайт другого зображення та запустіть анімацію "SwitchFirst"
        if (!isSwitched)
        {
            image2.sprite = sprite;
            animator.SetTrigger("SwitchFirst");
        }
        // Якщо зараз відображається друге зображення, установіть спрайт першого зображення та запустіть анімацію "SwitchSecond"
        else
        {
            image1.sprite = sprite;
            animator.SetTrigger("SwitchSecond");
        }
        // Міняємо значення змінної isSwitched
        isSwitched = !isSwitched;
    }

    // Метод SetImage використовується для встановлення спрайту поточного зображення
    public void SetImage(Sprite sprite)
    {
        // Якщо зараз відображається перше зображення, установлюємо його спрайт
        if (!isSwitched)
        {
            image1.sprite = sprite;
        }
        // Якщо наразі відображається друге зображення, установлюємо його спрайт
        else
        {
            image2.sprite = sprite;
        }
    }

    // Метод SyncImages використовується для синхронізації спрайтів двох компонентів Image
    public void SyncImages()
    {
        // Якщо зараз відображається перше зображення, встановлюємо спрайт другого зображення таким самим, як і перше зображення
        if (!isSwitched)
        {
            image2.sprite = image1.sprite;
        }
        // Якщо зараз відображається друге зображення, встановлюємо спрайт першого зображення таким самим, як і другого зображення
        else
        {
            image1.sprite = image2.sprite;
        }
    }
    // Метод GetImage використовується для отримання спрайту зображення, що відображається на даний момент
    public Sprite GetImage()
    {
        if (!isSwitched)
        {
            return image1.sprite;
        }
        else
        {
            return image2.sprite;
        }
    }
}
