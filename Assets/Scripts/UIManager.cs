using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI менеджер - отображает жизни игрока и другие элементы интерфейса.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Health Display")]
    [SerializeField] private Image[] healthIcons;
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;
    
    [Header("Boss Health")]
    [SerializeField] private GameObject bossHealthPanel;
    [SerializeField] private Slider bossHealthSlider;
    
    [Header("Level Complete")]
    [SerializeField] private GameObject levelCompletePanel;
    
    private PlayerController player;
    
    private void Start()
    {
        // Find player
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        
        if (player != null)
        {
            player.OnHealthChanged += UpdateHealthUI;
            UpdateHealthUI(player.CurrentHealth, 3); // Assuming max health is 3
        }
        
        // Hide boss health panel by default
        if (bossHealthPanel != null)
        {
            bossHealthPanel.SetActive(false);
        }
        
        // Hide level complete panel by default
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }
    }
    
    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnHealthChanged -= UpdateHealthUI;
        }
    }
    
    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthIcons == null || healthIcons.Length == 0) return;
        
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (i < currentHealth)
            {
                healthIcons[i].sprite = fullHeartSprite;
            }
            else
            {
                healthIcons[i].sprite = emptyHeartSprite;
            }
        }
    }
    
    public void ShowBossHealth(int currentHealth, int maxHealth)
    {
        if (bossHealthPanel != null)
        {
            bossHealthPanel.SetActive(true);
        }
        
        if (bossHealthSlider != null)
        {
            bossHealthSlider.maxValue = maxHealth;
            bossHealthSlider.value = currentHealth;
        }
    }
    
    public void HideBossHealth()
    {
        if (bossHealthPanel != null)
        {
            bossHealthPanel.SetActive(false);
        }
    }
    
    public void ShowLevelComplete()
    {
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
            Time.timeScale = 0f; // Pause game
        }
    }
    
    public void HideLevelComplete()
    {
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
            Time.timeScale = 1f; // Resume game
        }
    }
}
