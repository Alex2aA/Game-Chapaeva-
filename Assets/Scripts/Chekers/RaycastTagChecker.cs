using UnityEngine;

public class RaycastTagChecker : MonoBehaviour
{
    [Header("Настройки луча")]
    public float raycastDistance = 15.0f; // Максимальная дистанция луча
    public string targetTag = "playground"; // Тег для проверки

    //public static RaycastTagChecker Instance { get; private set; }

    public static bool isDrag = false;

    private Rigidbody rb;

    private void Awake()
    {
        //Instance = this;
    }

    void Start()
    {
         rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Debug.Log("aaa" + isDrag);
        if (!isDrag)
        {
            //Debug.Log("bbb" + isDrag);
            Ray();
        }
    }

    private void Ray()
    {
        // Создаем луч вниз из текущей позиции объекта
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        // Визуализация луча в редакторе
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red);

        // Проверяем луч
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Проверяем тег объекта
            if (hit.collider.CompareTag(targetTag))
            {
                //Debug.Log($"Объект {hit.collider.name} имеет тег {targetTag}");
                
                // Дополнительные действия при попадании в объект с нужным тегом
                OnTargetTagHit(hit);
            }
            else
            {
                //Debug.Log($"Объект {hit.collider.name} имеет тег {hit.collider.tag} вместо {targetTag}");
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
            ScoreManager.Instance.ChangeScore(this.gameObject);
            Destroy(gameObject);
            Debug.Log("Луч не попал ни в один объект");
        }
    }

    // Метод для дополнительных действий при попадании в объект с нужным тегом
    private void OnTargetTagHit(RaycastHit hit)
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }
}