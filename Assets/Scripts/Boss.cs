using UnityEngine;
using System.Collections;

/// <summary>
/// Босс с запасом здоровья и простой атакой.
/// </summary>
public class Boss : MonoBehaviour
{
    [Header("Boss Settings")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int damage = 1;
    
    [Header("Patrol")]
    [SerializeField] private Transform leftPatrolPoint;
    [SerializeField] private Transform rightPatrolPoint;
    
    [Header("Audio")]
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    
    // Components
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    // State
    private int currentHealth;
    private bool isFacingRight = true;
    private bool isDead = false;
    private bool canAttack = true;
    private Transform player;
    
    public delegate void BossHealthChanged(int currentHealth, int maxHealth);
    public event BossHealthChanged OnBossHealthChanged;
    
    public delegate void BossDefeated();
    public event BossDefeated OnBossDefeated;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Start()
    {
        currentHealth = maxHealth;
        UpdateBossHealthUI();
        
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
    
    private void Update()
    {
        if (isDead || player == null) return;
        
        Patrol();
        
        // Check if player is in attack range
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange && canAttack)
        {
            StartCoroutine(Attack());
        }
    }
    
    private void Patrol()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = isFacingRight ? moveSpeed : -moveSpeed;
        rb.velocity = velocity;
        
        // Check if reached patrol point
        if (isFacingRight && transform.position.x >= rightPatrolPoint.position.x)
        {
            Flip();
        }
        else if (!isFacingRight && transform.position.x <= leftPatrolPoint.position.x)
        {
            Flip();
        }
    }
    
    private IEnumerator Attack()
    {
        canAttack = false;
        
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
        PlaySound(attackSound);
        
        // Deal damage after a short delay
        yield return new WaitForSeconds(0.3f);
        
        // Check if player is still in range
        if (player != null && Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }
        }
        
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead) return;
        
        currentHealth -= damage;
        UpdateBossHealthUI();
        PlaySound(hurtSound);
        
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        isDead = true;
        PlaySound(deathSound);
        
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }
        
        OnBossDefeated?.Invoke();
        
        // Disable collider and schedule destruction
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        
        Destroy(gameObject, 2f);
    }
    
    private void UpdateBossHealthUI()
    {
        OnBossHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            // Check if player is falling from above
            if (playerController.transform.position.y > transform.position.y + 0.5f &&
                playerController.GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                // Damage boss and bounce player
                TakeDamage(1);
                
                Rigidbody2D playerRb = playerController.GetComponent<Rigidbody2D>();
                playerRb.velocity = new Vector2(playerRb.velocity.x, 7f);
            }
            else
            {
                // Damage player
                playerController.TakeDamage(damage);
            }
        }
    }
}
