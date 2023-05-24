using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class Portal : MonoBehaviour
{
    public Sprite[] sprites;
    public float animationSpeed = 0.1f;
    private SpriteRenderer spriteRenderer;
    public bool isAnimating = false;
    private float radius = 4f;

    public GameObject player; // Додайте гравця в інспекторі Unity

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isAnimating)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= radius)
            {

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                SaveManager.ClearSavedGame();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
