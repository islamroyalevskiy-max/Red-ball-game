using UnityEngine;

/// <summary>
/// Базовый класс для всех врагов в игре.
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected Transform groundCheckPoint;
    [SerializeField] protected float groundCheckRadius = 0.2f;
    [SerializeField] protected LayerMask groundLayer;
    
    [Header("Patrol Points")]
    [SerializeField] protected Transform leftPatrolPoint;
    [SerializeField] protected Transform rightPatrolPoint;
    
    // Components
    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    
    // State
    protected bool isFacingRight = true;
    protected bool isDead = false;
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    protected virtual void Update()
    {
        if (isDead) return;
        
        Patrol();
    }
    
    protected virtual void Patrol()
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
        
        // Check if about to fall off edge
        bool isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        if (!isGrounded)
        {
            Flip();
        }
    }
    
    protected virtual void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    public virtual void TakeDamageFromAbove()
    {
        if (isDead) return;
        
        Die();
    }
    
    protected virtual void Die()
    {
        isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }
        
        // Disable collider and schedule destruction
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        
        Destroy(gameObject, 1f);
    }
    
    public bool IsDead => isDead;
    
    // Debug visualization
    protected virtual void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
        
        if (leftPatrolPoint != null && rightPatrolPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(leftPatrolPoint.position, rightPatrolPoint.position);
        }
    }
}
