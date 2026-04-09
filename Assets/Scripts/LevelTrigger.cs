using UnityEngine;

/// <summary>
/// Триггер перехода на следующий уровень.
/// </summary>
public class LevelTrigger : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private string nextLevelName;
    [SerializeField] private bool showLevelComplete = true;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            if (showLevelComplete)
            {
                // Show level complete UI
                UIManager uiManager = FindObjectOfType<UIManager>();
                if (uiManager != null)
                {
                    uiManager.ShowLevelComplete();
                }
                
                // Load next level after delay
                Invoke(nameof(LoadNextLevel), 2f);
            }
            else
            {
                LoadNextLevel();
            }
        }
    }
    
    private void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelName);
        }
    }
}
