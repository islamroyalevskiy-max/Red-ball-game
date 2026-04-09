using UnityEngine;

/// <summary>
/// Менеджер анимации игрока - управляет параметрами Animator.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    
    // Animation parameters
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int HurtHash = Animator.StringToHash("Hurt");
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        // Update running state
        bool isRunning = Mathf.Abs(rb.velocity.x) > 0.1f;
        animator.SetBool(IsRunningHash, isRunning);
        
        // Update grounded state
        // Note: This should be set by PlayerController
        // animator.SetBool(IsGroundedHash, isGrounded);
    }
    
    public void SetGrounded(bool grounded)
    {
        animator.SetBool(IsGroundedHash, grounded);
    }
    
    public void TriggerJump()
    {
        animator.SetTrigger(JumpHash);
    }
    
    public void TriggerHurt()
    {
        animator.SetTrigger(HurtHash);
    }
}
