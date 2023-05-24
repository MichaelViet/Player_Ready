using UnityEngine;
using UnityEngine.SceneManagement;

public class BackPortal : MonoBehaviour
{
    public float interactionRadius = 5f;
    public LayerMask playerLayer;
    private bool isPlayerInRange = false;

    private void Update()
    {
        isPlayerInRange = Physics.CheckSphere(transform.position, interactionRadius, playerLayer);
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SaveManager.ClearSavedGame();
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
