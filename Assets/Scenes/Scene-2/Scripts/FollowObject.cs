using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public List<Transform> objects;
    public Vector3 offset;

    // Додаємо два нові поля для налаштування позиції об'єкта відносно об'єкта, який він слідкує
    [Range(-1f, 1f)] public float xPositionFactor = 0f;
    [Range(-1f, 1f)] public float yPositionFactor = 0f;

    void Update()
    {
        if (objects != null && objects.Count > 0)
        {
            Vector3 averagePosition = GetAveragePosition(objects);
            // Використовуйте фактори позиції для налаштування розташування об'єкта
            Vector3 newPosition = averagePosition + offset;
            newPosition.x += xPositionFactor * (averagePosition.x / 2);
            newPosition.y += yPositionFactor * (averagePosition.y / 2);
            transform.position = newPosition;
        }
    }
    private Vector3 GetAveragePosition(List<Transform> objects)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (Transform obj in objects)
        {
            if (obj != null)
            {
                sum += obj.position;
                count++;
            }
        }

        return count > 0 ? sum / count : Vector3.zero;
    }

}
