using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Менеджер сцены - управляет переходами между уровнями и состоянием игры.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string nextLevelScene;
    
    [Header("Player Spawn")]
    [SerializeField] private Transform playerSpawnPoint;
    
    public static LevelManager Instance { get; private set; }
    
    private GameObject player;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void Start()
    {
        // Find player in scene
        player = GameObject.FindGameObjectWithTag("Player");
        
        // Subscribe to player death event
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.OnPlayerDied += OnPlayerDied;
            }
        }
    }
    
    private void OnDestroy()
    {
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.OnPlayerDied -= OnPlayerDied;
            }
        }
    }
    
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    
    public void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelScene))
        {
            LoadLevel(nextLevelScene);
        }
    }
    
    public void LoadMainMenu()
    {
        LoadLevel(mainMenuScene);
    }
    
    public void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().name);
    }
    
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
    
    private void OnPlayerDied()
    {
        // Wait a moment then restart level
        Invoke(nameof(RestartLevel), 1.5f);
    }
    
    public Vector3 GetPlayerSpawnPoint()
    {
        return playerSpawnPoint != null ? playerSpawnPoint.position : Vector3.zero;
    }
}
