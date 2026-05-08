using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;



public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    float horizontalMovement;
    bool isFacingRight = true;

    [Header("Jump")]
    public float jumpPower = 5f;
    public int maxJump = 2;
    int jumpsRemaining;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize;
    public LayerMask groundLayer;
    bool isGrounded;

    [Header("Wall Check")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize;
    public LayerMask wallLayer;
    bool isTouchingWall;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float fallMultiplier = 2f;
    public float maxFallSpeed = 12f;

    [Header("Wall Slide")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding;

    [Header("Wall Jump")]
    public Vector2 wallJumpPower = new Vector2(8f, 12f);
    public float wallJumpDuration = 0.2f;
    bool isWallJumping;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;

    bool isDashing;
    bool canDash = true;
    public bool canUseDash = true;

    void Start()
    {
        jumpsRemaining = maxJump;
    }

    void Update()
    {
        GroundCheck();
        WallCheck();
        HandleGravity();
        HandleWallSlide();

        if ((Input.GetMouseButtonDown(1) || Keyboard.current.fKey.wasPressedThisFrame)
            && canDash && !isDashing && canUseDash)
        {
            StartCoroutine(Dash());
        }

        if (!isWallJumping && !isDashing)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        }

        Flip();
    }

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);

        // Reset seulement quand on touche vraiment le sol en descendant
        if (isGrounded && rb.linearVelocity.y <= 0)
        {
            jumpsRemaining = maxJump;
        }
    }

    void WallCheck()
    {
        isTouchingWall = Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    void HandleGravity()
    {
        if (isDashing) return;

        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    void HandleWallSlide()
    {
        if (isTouchingWall && !isGrounded && horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // Wall jump prioritaire
        if (isWallSliding)
        {
            isWallJumping = true;
            rb.linearVelocity = Vector2.zero;

            float direction = isFacingRight ? -1 : 1;
            rb.linearVelocity = new Vector2(direction * wallJumpPower.x, wallJumpPower.y);

            Invoke(nameof(StopWallJump), wallJumpDuration);
            return;
        }

        // Jump limité par le compteur
        if (jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            jumpsRemaining--;
        }
    }

    System.Collections.IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float direction = isFacingRight ? 1 : -1;
        rb.linearVelocity = new Vector2(direction * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void DashInput(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && canUseDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    void StopWallJump()
    {
        isWallJumping = false;
    }

    void Flip()
    {
        if ((isFacingRight && horizontalMovement < 0) || (!isFacingRight && horizontalMovement > 0))
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
