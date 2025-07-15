using UnityEngine;

public class DragShoot : MonoBehaviour
{

    [SerializeField] private TurnManager.PlayerTurn _color;

    public TurnManager.PlayerTurn __color => _color;

    [Tooltip("Минимальный интервал между звуками (секунды)")]
    public float soundCooldown = 0.3f;

    [Tooltip("Минимальная сила удара для воспроизведения")]
    public float minImpactForce = 0.5f;

    private float lastPlayTime;

    private Vector3 startMousePos;
    private Vector3 releaseMousePos;
    private float startTime;
    private bool isDragging = false;
    private Rigidbody rb;
    private float startY;
    private Vector3 originalPosition;
    private Collider col;

    [Header("Настройки силы")]
    public float maxForce = 30f;
    public float maxDragDistance = 1f;
    public float forceDecayRate = 0.5f;
    public float maxHoldTime = 2f; // Максимальное время удержания
    public PhysicsMaterial lowFrictionMaterial; // Физический материал с низким трение

    private void Awake()
    {
        lastPlayTime = -soundCooldown; // Чтобы первый звук мог проиграться сразу   
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        startY = transform.position.y;
        originalPosition = transform.position;

        // Настраиваем физические свойства
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        rb.linearDamping = 2f; // Сопротивление движению
        rb.angularDamping = 10f;

        // Применяем физический материал с низким трением
        if (lowFrictionMaterial != null)
        {
            col.material = lowFrictionMaterial;
        }
        else
        {
            Debug.LogWarning("Не назначен физический материал с низким трением!");
        }
    }

    void OnMouseDown()
    {
        if (rb != null
            && TurnManager.Instance.CanStrike()
            && TurnManager.Instance.CurrentTurn == _color)
        {
            isDragging = true;
            RaycastTagChecker.isDrag = true;
            startTime = Time.time;
            startMousePos = GetMouseWorldPosition();
            startMousePos.y = startY;
            rb.isKinematic = true;

            // Сохраняем оригинальную позицию для возврата
            originalPosition = transform.position;
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 currentMousePos = GetMouseWorldPosition();
            currentMousePos.y = startY;

            // Рассчитываем смещение только в плоскости XZ
            Vector3 offset = currentMousePos - startMousePos;
            offset.y = 0;

            // Ограничиваем расстояние перемещения
            Vector3 newPosition = startMousePos + Vector3.ClampMagnitude(offset, maxDragDistance);
            newPosition.y = startY;

            transform.position = newPosition;
        }
    }

    void OnMouseUp()
    {
        if (isDragging && rb != null)
        {
            isDragging = false;
            Invoke("Check", 0.2f);
            releaseMousePos = GetMouseWorldPosition();
            releaseMousePos.y = startY;
            rb.isKinematic = false;

            // Рассчитываем вектор силы только в плоскости XZ
            Vector3 forceVector = originalPosition - transform.position;
            forceVector.y = 0;

            // Учет времени удержания
            float holdDuration = Time.time - startTime;

            // Ограничиваем максимальное время удержания
            float normalizedHoldTime = Mathf.Clamp01(holdDuration / maxHoldTime);

            // Коэффициент затухания силы
            float timeDecay = 1f - normalizedHoldTime;

            // Рассчитываем силу броска
            float forceMagnitude = Mathf.Clamp(forceVector.magnitude, 0, maxDragDistance) * maxForce * timeDecay;

            // Применяем силу в направлении от текущей позиции к оригинальной
            rb.AddForce(forceVector.normalized * forceMagnitude, ForceMode.Impulse);

            TurnManager.Instance.OnCheckerStruck();
        }
    }

    private void Check()
    {
        RaycastTagChecker.isDrag = false;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        // Создаем плоскость на уровне объекта (параллельно XZ)
        Plane plane = new Plane(Vector3.up, new Vector3(0, startY, 0));

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= minImpactForce &&
            Time.time - lastPlayTime >= soundCooldown)
        {
            SoundStrike.Instance.PlayRandomCollisionSound();
            lastPlayTime = Time.time;
        }
    }
}



public enum PlayerColor
{
    White,
    Black
}