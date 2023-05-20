using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour
{
    Transform target; // Поточна ціль для руху гравця
    NavMeshAgent agent; // Компонент NavMeshAgent для управління рухом гравця

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Отримуємо посилання на NavMeshAgent
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position); // Встановлюємо пункт призначення агента на позицію цілі
            FaceTarget(); // Обертаємо гравця в бік цілі
        }
    }

    public void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point); // Рухатися до заданої точки
    }

    public void FollowTarget(Interactable newTarget)
    {
        agent.stoppingDistance = newTarget.radius * .8f; // Встановлюємо зупинкову відстань агента
        agent.updateRotation = false; // Вимикаємо автоматичне оновлення повороту агента

        target = newTarget.interactionTransform; // Встановлюємо нову ціль для переслідування
    }

    public void StopFollowingTarget()
    {
        agent.stoppingDistance = 0f; // Встановлюємо зупинкову відстань агента на 0
        agent.updateRotation = true; // Увімкнення автоматичного оновлення повороту агента

        target = null; // Зупиняємо переслідування цілі
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized; // Визначаємо вектор напрямку до цілі
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z)); // Обчислюємо обертання, щоб дивитися в бік цілі
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Плавно обертаємо гравця у бік цілі
    }
}
