using UnityEngine;

/// <summary>
/// Скрипт камеры, которая плавно следует за игроком.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("Персонаж, за которым будет следовать камера")]
    [SerializeField] private Transform target;

    [Header("Position Settings")]
    [Tooltip("Смещение камеры относительно игрока по оси X")]
    [SerializeField] private float xOffset = 0f;
    [Tooltip("Смещение камеры относительно игрока по оси Y")]
    [SerializeField] private float yOffset = 1f;
    [Tooltip("Смещение камеры по оси Z (для 2D обычно -10)")]
    [SerializeField] private float zOffset = -10f;

    [Header("Smooth Follow")]
    [Tooltip("Скорость плавного следования камеры (меньше = плавнее)")]
    [SerializeField] private float smoothSpeed = 0.125f;
    [Tooltip("Если включено, камера будет следовать строго по позиции без сглаживания")]
    [SerializeField] private bool hardFollow = false;

    [Header("Bounds (Optional)")]
    [Tooltip("Минимальная позиция камеры по X (0 = без ограничений)")]
    [SerializeField] private float minX = 0f;
    [Tooltip("Максимальная позиция камеры по X (0 = без ограничений)")]
    [SerializeField] private float maxX = 0f;
    [Tooltip("Минимальная позиция камеры по Y (0 = без ограничений)")]
    [SerializeField] private float minY = 0f;
    [Tooltip("Максимальная позиция камеры по Y (0 = без ограничений)")]
    [SerializeField] private float maxY = 0f;

    [Header("Dead Zone (Optional)")]
    [Tooltip("Зона вокруг центра, где камера не двигается (0 = отключено)")]
    [SerializeField] private float deadZoneWidth = 0f;
    [Tooltip("Зона вокруг центра, где камера не двигается (0 = отключено)")]
    [SerializeField] private float deadZoneHeight = 0f;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        // Если цель не назначена, пытаемся найти игрока автоматически
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                Debug.Log("CameraFollow: Цель найдена автоматически по тегу 'Player'");
            }
            else
            {
                player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                    Debug.Log("CameraFollow: Цель найдена автоматически по тегу 'Player'");
                }
                else
                {
                    Debug.LogWarning("CameraFollow: Цель не назначена и игрок не найден!");
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Вычисляем желаемую позицию камеры
        Vector3 desiredPosition = new Vector3(
            target.position.x + xOffset,
            target.position.y + yOffset,
            zOffset
        );

        // Применяем dead zone если включена
        if (deadZoneWidth > 0 || deadZoneHeight > 0)
        {
            Vector3 currentPos = transform.position;
            
            if (deadZoneWidth > 0)
            {
                if (Mathf.Abs(desiredPosition.x - currentPos.x) < deadZoneWidth)
                {
                    desiredPosition.x = currentPos.x;
                }
            }
            
            if (deadZoneHeight > 0)
            {
                if (Mathf.Abs(desiredPosition.y - currentPos.y) < deadZoneHeight)
                {
                    desiredPosition.y = currentPos.y;
                }
            }
        }

        // Ограничиваем позицию камеры границами уровня
        if (minX != 0 || maxX != 0)
        {
            float x = desiredPosition.x;
            if (minX != 0 && x < minX) x = minX;
            if (maxX != 0 && x > maxX) x = maxX;
            desiredPosition.x = x;
        }

        if (minY != 0 || maxY != 0)
        {
            float y = desiredPosition.y;
            if (minY != 0 && y < minY) y = minY;
            if (maxY != 0 && y > maxY) y = maxY;
            desiredPosition.y = y;
        }

        // Плавно перемещаем камеру к цели
        if (hardFollow)
        {
            transform.position = desiredPosition;
        }
        else
        {
            Vector3 smoothedPosition = Vector3.SmoothDamp(
                transform.position, 
                desiredPosition, 
                ref velocity, 
                smoothSpeed
            );
            transform.position = smoothedPosition;
        }
    }

    // Метод для установки цели во время выполнения
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Метод для установки цели по GameObject
    public void SetTarget(GameObject newTarget)
    {
        if (newTarget != null)
        {
            target = newTarget.transform;
        }
    }

    // Отладочная визуализация
    private void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position);
        }

        // Визуализация границ
        if (minX != 0 || maxX != 0 || minY != 0 || maxY != 0)
        {
            Gizmos.color = Color.red;
            Vector3 center = new Vector3(
                (minX + maxX) / 2,
                (minY + maxY) / 2,
                transform.position.z
            );
            Vector3 size = new Vector3(
                Mathf.Abs(maxX - minX),
                Mathf.Abs(maxY - minY),
                0
            );
            Gizmos.DrawWireCube(center, size);
        }
    }
}
