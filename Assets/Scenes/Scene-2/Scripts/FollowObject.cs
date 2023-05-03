using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform Object;
    public Vector3 offset;

    // Додайте два нові поля для налаштування позиції об'єкта відносно об'єкта, який він слідкує
    [Range(-1f, 1f)] public float xPositionFactor = 0f;
    [Range(-1f, 1f)] public float yPositionFactor = 0f;

    void Update()
    {
        if (Object != null)
        {
            // Використовуйте фактори позиції для налаштування розташування об'єкта
            Vector3 newPosition = Object.position + offset;
            newPosition.x += xPositionFactor * (Object.localScale.x / 2);
            newPosition.y += yPositionFactor * (Object.localScale.y / 2);
            transform.position = newPosition;
        }
    }
}
