using UnityEngine;
using UnityEngine.Splines;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerInput input;
    private PlayerCrouch crouch;
    private PlayerHealth health;
    private SpriteRenderer sprite;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<PlayerHealth>();
        crouch = GetComponent<PlayerCrouch>();
        sprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        //Устанавливает парраметр speed в Animator модулем 
        animator.SetFloat("Speed", Mathf.Abs(input.Move));

        //Устанавливает булевые парраметры 
        animator.SetBool("IsRun",input.RunHeld);
        animator.SetBool("IsCrouch", crouch != null && crouch.IsCrouching);
        animator.SetBool("IsDead",health!= null && health.CurrentHP <= 0);
        animator.SetBool("IsJump", input.JumpPressed);
    }


    private void LateUpdate()
    {
        //Флип спрайт
        if (rb.linearVelocity.x > 0.1f)
        {
            sprite.flipX = true;
        }
        else if (rb.linearVelocity.x < -0.1f)
        {
            sprite.flipX = false;
        }
    }
}
