using UnityEngine;

public class FollowWizard : MonoBehaviour
{
    public Transform wizard;
    public Vector3 offset;

    void Update()
    {
        if (wizard != null)
        {
            transform.position = wizard.position + offset;
        }
    }
}
