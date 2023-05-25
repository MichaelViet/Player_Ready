using UnityEngine;

public class BossMusic : MonoBehaviour
{
    public AudioClip nextMusic;
    public bool hasMusicChanged = false;
    public float radius = 5f;
    public GameObject player;
    private LevelThreeController levelThreeController;

    private void Start()
    {
        levelThreeController = FindObjectOfType<LevelThreeController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !hasMusicChanged)
        {
            levelThreeController.levelMusic = nextMusic;
            levelThreeController.audioController.PlayAudio(levelThreeController.levelMusic, null, null);
            hasMusicChanged = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
