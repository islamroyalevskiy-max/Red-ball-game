using UnityEngine;

/// <summary>
/// Обычный враг - умирает от прыжка сверху.
/// </summary>
public class NormalEnemy : Enemy
{
    [Header("Normal Enemy Settings")]
    [SerializeField] private int damage = 1;
    
    private PlayerController player;
    
    protected override void Awake()
    {
        base.Awake();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            // Check if player is falling from above
            if (player.transform.position.y > transform.position.y + 0.5f && 
                player.GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                // Kill enemy
                TakeDamageFromAbove();
                
                // Bounce player
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                playerRb.velocity = new Vector2(playerRb.velocity.x, 5f);
            }
            else
            {
                // Damage player
                player.TakeDamage(damage);
            }
        }
    }
}
