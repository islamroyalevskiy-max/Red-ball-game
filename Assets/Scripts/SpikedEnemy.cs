using UnityEngine;

/// <summary>
/// Враг с шипами - наносит урон при любом касании, не может быть убит прыжком сверху.
/// </summary>
public class SpikedEnemy : Enemy
{
    [Header("Spiked Enemy Settings")]
    [SerializeField] private int damage = 1;
    
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
            // Always damage player, cannot be killed by jumping on top
            player.TakeDamage(damage);
        }
    }
    
    // Override to prevent death from above
    public override void TakeDamageFromAbove()
    {
        // Spiked enemies cannot be killed by jumping on them
        // They deal damage instead
    }
}
