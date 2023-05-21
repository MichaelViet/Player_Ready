using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public GameObject player;
    void Awake()
    {
        instance = this;
    }

    public void KillPlayer()
    {
        // Перезавантаження поточної сцени
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
