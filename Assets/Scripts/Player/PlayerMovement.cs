using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public PlayerData playerData;
    public PlayerInput input;
    public Rigidbody2D rb;
    public float moveDirectionVector = 1;

    void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    public void FixedUpdate()
    {

        if (playerData.isDashing)
        {
            rb.gravityScale = 0;
        }
        else if (!playerData.isGrounded)
        {

            if (rb.linearVelocityY <= 0)
            {
                rb.gravityScale = playerData.fallingGravityScale;
                // For capping max falling speeds
                rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -playerData.maxFallSpeed));
            }
        }
        else if (playerData.isGrounded)
        {
            playerData.isJumping = false;
            rb.gravityScale = playerData.defaultGravityScale;
        }
        // Extend air time slightly between threshold
        else if (playerData.isJumping && Mathf.Abs(rb.linearVelocityY) < playerData.jumpHangTimeThreshold)
            rb.gravityScale = playerData.jumpHangGravityScale;

    }

    public void HandleMovement()
    {
        if (playerData.isMovementDisabled) return;

        HandleFaceDirection();
        HandleJump();

        if (input.wasdInputVector.WasReleasedThisFrame())
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        }
        if (input.wasdInputVector.IsPressed())
        {
            // W+A = 0.707 x 
            moveDirectionVector = Mathf.Round(input.wasdInputVector.ReadValue<Vector2>().x);

            //if (input.wasdInputVector.ReadValue<Vector2>().y <= 0)
            rb.linearVelocity = new Vector2(playerData.targetSpeed * moveDirectionVector, rb.linearVelocityY);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        }
        if (input.jumpInput.ReadValue<float>() >= 1)
            playerData.isJumping = true;

        HandleDash();
        HandleAnimations();
    }
    public void HandleAnimations()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocityX));
        animator.SetFloat("VerticalSpeed", rb.linearVelocityY);
        animator.SetBool("isGrounded", playerData.isGrounded);
    }

    public void HandleFaceDirection()
    {
        if (animator.GetBool("IsAttacking")) return;
        Vector3 vector3scale = this.transform.localScale;
        if (Mathf.Abs(moveDirectionVector) >= 1)
            this.transform.localScale = new Vector3(moveDirectionVector * Mathf.Abs(vector3scale.x), vector3scale.y, vector3scale.z);
    }

    public void HandleJump()
    {
        if (input.jumpInput.WasReleasedThisFrame() && !playerData.isGrounded)
        {
            float linearYtoSet = rb.linearVelocityY > 0 ? 0 : rb.linearVelocityY;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, linearYtoSet);
        }

        if (input.jumpInput.WasPressedThisFrame())
        {
            if (playerData.isGrounded)
            {
                rb.AddForce(new Vector2(0, playerData.jumpForce), ForceMode2D.Impulse);
                playerData.isJumping = true;
                playerData.isGrounded = false;
            }
        }

    }

    public void HandleDash()
    {
        if (playerData.isDashing) return;
        if (playerData.dashCooldownTimer > 0) return;
        if (input.dashInput.WasPressedThisFrame())
        {
            playerData.isMovementDisabled = true;
            playerData.isDashing = true;
            playerData.isInvincible = true;
            playerData.dashCooldownTimer = playerData.dashCooldown;
            animator.SetTrigger("Dash");
            //rb.linearVelocity = Vector2.zero;
            rb.linearVelocity = moveDirectionVector * Vector2.right * playerData.dashForce;
            StartCoroutine(StopTeleportDash());
        }
    }

    public IEnumerator StopTeleportDash()
    {
        yield return new WaitForSeconds(playerData.dashDuration);
        animator.SetTrigger("DashEnd");
        rb.linearVelocity = Vector2.zero; //new Vector2(0, rb.linearVelocityY);
        playerData.isMovementDisabled = false;
        playerData.isDashing = false;
        playerData.isInvincible = false;
    }
    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (
    //        collision.gameObject.CompareTag("Ground") ||
    //        collision.gameObject.CompareTag("Platform")
    //    )
    //    {
    //        playerData.isJumping = false;
    //        playerData.isGrounded = true;
    //    }
    //}

    //public void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (
    //        collision.gameObject.CompareTag("Ground") ||
    //        collision.gameObject.CompareTag("Platform")
    //    )
    //    {
    //        playerData.isGrounded = false;
    //    }
    //}

}
