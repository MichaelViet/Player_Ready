using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Portal : MonoBehaviour
{
    public Sprite[] sprites; // Загрузіть ваші спрайти тут в інспекторі Unity
    public float animationSpeed = 0.1f; // Швидкість анімації (час між кадрами)
    private SpriteRenderer spriteRenderer;
    public bool isAnimating = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Викликайте цей метод для початку анімації
    public void StartAnimation()
    {
        if (!isAnimating)
        {
            StartCoroutine(AnimateSprites());
        }
    }

    private IEnumerator AnimateSprites()
    {
        isAnimating = true;

        while (true)
        {
            foreach (var sprite in sprites)
            {
                spriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(animationSpeed);
            }
        }
    }
}
