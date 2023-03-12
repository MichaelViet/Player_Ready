using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string nextSceneName;
    private bool hasChangedScene = false;

    void Update()
    {
        if (!hasChangedScene && Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene(nextSceneName);
            hasChangedScene = true;
        }
    }
}
