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

    public GameObject player; // Add the player in the Unity inspector

    private int currentSpriteIndex = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Scene-4")
        {
            radius = 0f; // Set radius to 0 for Scene-4
            StartAnimation();
        }
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
            spriteRenderer.sprite = sprites[currentSpriteIndex];
            currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length; // Increment index and wrap around

            yield return new WaitForSeconds(animationSpeed);
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
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
