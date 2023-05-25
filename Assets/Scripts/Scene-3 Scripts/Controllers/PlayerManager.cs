using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public GameObject player;
    public GameObject deathObject;
    void Awake()
    {
        instance = this;
    }

    public void KillPlayer()
    {
        // Перезавантаження поточної сцени
        deathObject.SetActive(true);
        Time.timeScale = 0f;
    }
}
