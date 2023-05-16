using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public float speed = 1.5f;
    public int damage = 20;
    public Rigidbody2D rb;
    public GameObject impactEffectPrefab;
    public AudioClip shootSound;
    private AudioSource audioSource;
    private ObjectPool<GameObject> impactEffectPool;
    public float impactEffectLifeTime = 2f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = shootSound;
        impactEffectPool = new ObjectPool<GameObject>(() => Instantiate(impactEffectPrefab), null, DestroyObject); // Ініціалізуємо пул impactEffectPrefab
    }

    private void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }

    void Start()
    {
        rb.velocity = transform.right * speed;
        // Відтворити звук вистрілу
        audioSource.Play();
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Boss"))
        {
            hitInfo.GetComponent<Boss>().health -= damage; // Зменшуємо здоров'я боса

            // Отримуємо об'єкт ефекту удару з пулу
            GameObject impactEffect = impactEffectPool.Get();
            impactEffect.transform.position = transform.position;
            impactEffect.transform.rotation = transform.rotation;
            impactEffect.SetActive(true);

            // Знищуємо ефект удару через визначений час
            Destroy(impactEffect, impactEffectLifeTime);
            Destroy(gameObject);
        }
        else if (hitInfo.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            // Отримуємо об'єкт ефекту удару з пулу
            GameObject impactEffect = impactEffectPool.Get();
            impactEffect.transform.position = transform.position;
            impactEffect.transform.rotation = transform.rotation;
            impactEffect.SetActive(true);

            // Знищуємо ефект удару через визначений час
            Destroy(impactEffect, impactEffectLifeTime);
            Destroy(gameObject);
        }
        else if (hitInfo.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Отримуємо об'єкт ефекту удару з пулу
            GameObject impactEffect = impactEffectPool.Get();
            impactEffect.transform.position = transform.position;
            impactEffect.transform.rotation = transform.rotation;
            impactEffect.SetActive(true);
            // Знищуємо ефект удару через визначений час
            Destroy(impactEffect, impactEffectLifeTime);
            Destroy(gameObject);
        }
    }
}
