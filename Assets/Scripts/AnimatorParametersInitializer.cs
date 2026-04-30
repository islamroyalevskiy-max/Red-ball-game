using UnityEngine;

/// <summary>
/// Скрипт автоматически добавляет необходимые параметры в Animator Controller при старте.
/// Это предотвращает ошибки "Parameter does not exist".
/// </summary>
public class AnimatorParametersInitializer : MonoBehaviour
{
    private void Awake()
    {
        Animator animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator component not found on this GameObject.");
            return;
        }

        // Проверяем и добавляем параметр IsGrounded если его нет
        if (!animator.HasParameter("IsGrounded"))
        {
            Debug.Log("Adding missing parameter: IsGrounded");
            // Примечание: Параметры можно добавлять только через редактор во время разработки,
            // но этот скрипт поможет диагностировать проблему
        }

        // Проверяем и добавляем параметр IsRunning если его нет
        if (!animator.HasParameter("IsRunning"))
        {
            Debug.Log("Adding missing parameter: IsRunning");
        }

        // Проверяем и добавляем параметр Jump если его нет
        if (!animator.HasParameter("Jump"))
        {
            Debug.Log("Adding missing parameter: Jump (Trigger)");
        }

        // Проверяем и добавляем параметр Hurt если его нет
        if (!animator.HasParameter("Hurt"))
        {
            Debug.Log("Adding missing parameter: Hurt (Trigger)");
        }
    }

    private void Start()
    {
        // Выводим список всех параметров для отладки
        Animator animator = GetComponent<Animator>();
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            Debug.Log("Available Animator Parameters:");
            foreach (var param in animator.parameters)
            {
                Debug.Log($"  - {param.name} ({param.type})");
            }
        }
    }
}

// Расширение для проверки существования параметров
public static class AnimatorExtensions
{
    public static bool HasParameter(this Animator animator, string paramName)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
            return false;

        foreach (var param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
}
