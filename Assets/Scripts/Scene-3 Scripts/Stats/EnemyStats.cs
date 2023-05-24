using UnityEngine;

public class EnemyStats : CharacterStats
{
    public bool isBoss = false;
    public GameObject[] littleSkeletons;

    public override void Die()
    {
        base.Die();

        if (isBoss)
        {
            // Активувати об'єкти LittleSkeleton
            foreach (GameObject skeleton in littleSkeletons)
            {
                skeleton.SetActive(true);
            }
        }

        Destroy(gameObject);
    }
}
