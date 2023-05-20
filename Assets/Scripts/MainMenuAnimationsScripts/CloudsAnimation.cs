using System.Collections;
using UnityEngine;
public class CloudsAnimation : MonoBehaviour
{
    public GameObject clouds;
    public float speed = 1f;
    public float leftLimit = 0;
    public float rightLimit = 0;
    private Vector3 direction = Vector3.left;

    private void Start()
    {
        StartCoroutine(MoveClouds());
    }

    IEnumerator MoveClouds()
    {
        while (true)
        {
            clouds.transform.position += direction * speed * Time.deltaTime;

            if (clouds.transform.position.x <= leftLimit)
            {
                direction = Vector3.right;
                clouds.transform.position = new Vector3(leftLimit, clouds.transform.position.y, clouds.transform.position.z);
            }
            else if (clouds.transform.position.x >= rightLimit)
            {
                direction = Vector3.left;
                clouds.transform.position = new Vector3(rightLimit, clouds.transform.position.y, clouds.transform.position.z);
            }

            yield return null;
        }
    }
}
