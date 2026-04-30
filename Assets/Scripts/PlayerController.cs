using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Основной класс управления игроком с движением, прыжками и здоровьем.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    
    [Header("Audio")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    
    // Components
    private Rigidbody2D rb;
    private Animator animator;
    
    // State
    private bool isGrounded;
    private bool isFacingRight = true;
    private int currentHealth;
    private bool isInvincible = false;
    
    // Events
    public delegate void HealthChanged(int currentHealth, int maxHealth);
    public event HealthChanged OnHealthChanged;
    
    public delegate void PlayerDied();
    public event PlayerDied OnPlayerDied;
    
    // Properties
    public int CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
    
    private void Update()
    {
        if (IsDead) return;
        
        HandleMovement();
        HandleJump();
        UpdateAnimations();
    }
    
    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float velocityX = horizontalInput * moveSpeed;
        
        rb.velocity = new Vector2(velocityX, rb.velocity.y);
        
        // Flip sprite based on direction
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }
    
    private void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }
    
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        PlaySound(jumpSound);
        animator.SetTrigger("Jump");
    }
    
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    private void UpdateAnimations()
    {
        bool isMoving = Mathf.Abs(rb.velocity.x) > 0.1f;
        animator.SetBool("IsRunning", isMoving);
        animator.SetBool("IsGrounded", isGrounded);
    }
    
    public void TakeDamage(int damage)
    {
        if (isInvincible || IsDead) return;
        
        currentHealth -= damage;
        UpdateHealthUI();
        
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFrames());
            PlaySound(hurtSound);
            animator.SetTrigger("Hurt");
        }
    }
    
    private IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        
        float duration = 1.5f;
        float flashRate = 0.1f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashRate);
            elapsed += flashRate;
        }
        
        spriteRenderer.enabled = true;
        isInvincible = false;
    }
    
    private void Die()
    {
        PlaySound(deathSound);
        OnPlayerDied?.Invoke();
        gameObject.SetActive(false);
    }
    
    private void UpdateHealthUI()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
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
        // Check for spike collision
        if (collision.gameObject.CompareTag("Spike"))
        {
            TakeDamage(1);
        }
    }
    
    // Debug visualization
    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}
