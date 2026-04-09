using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Меню управления - главное меню и пауза.
/// </summary>
public class MenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    
    [Header("Settings")]
    [SerializeField] private string firstLevelScene = "World1_Level1";
    
    private bool isPaused = false;
    
    private void Start()
    {
        // Show main menu at start
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }
        
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        
        Time.timeScale = 0f; // Start paused in menu
    }
    
    private void Update()
    {
        // Toggle pause with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mainMenuPanel.activeSelf)
            {
                // Don't pause if in main menu
                return;
            }
            
            TogglePause();
        }
    }
    
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(firstLevelScene);
    }
    
    public void ContinueGame()
    {
        TogglePause();
    }
    
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void LoadMainMenu()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene("MainMenu");
    }
    
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
    
    private void TogglePause()
    {
        isPaused = !isPaused;
        
        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(true);
            }
        }
        else
        {
            Time.timeScale = 1f;
            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(false);
            }
        }
    }
}
