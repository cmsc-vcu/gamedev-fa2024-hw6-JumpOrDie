using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;              // Speed of the player movement
    public float jumpForce = 7f;              // Initial jump force
    public float maxJumpTime = 0.5f;          // Max time the player can hold jump for a higher jump
    public float variableJumpForce = 10f;     // Additional force applied when holding jump
    public float apexModifier = 0.8f;         // Multiplier for movement at the apex of the jump
    public float fallMultiplier = 1.5f;       // Multiplier for falling speed (snappier fall)
    public float jumpBufferTime = 0.2f;       // Time window for jump buffering (in seconds)
    public float coyoteTime = 0.2f;           // Time window for coyote time (in seconds)

    private bool isGrounded;
    private bool isJumping;
    private float jumpTimeCounter;            // Counter to track the jump hold duration
    private float jumpBufferCounter;          // Counter for tracking jump buffering
    private float coyoteTimeCounter;          // Counter for coyote time
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        HandleJump();
        ApplyJumpBuffer();
        ApplyCoyoteTime();
    }

    void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float movement = horizontalInput * moveSpeed;

        // Check if the player is at the apex of the jump
        if (Mathf.Abs(rb.velocity.y) < 0.1f && !isGrounded)  // Close to 0 indicates apex
        {
            movement *= apexModifier;  // Apply apex modifier to slow down horizontal movement slightly
        }

        rb.velocity = new Vector2(movement, rb.velocity.y);
    }

    void HandleJump()
    {
        // Start the jump if grounded or in coyote time, or if jump buffer allows it
        if ((Input.GetButtonDown("Jump") || jumpBufferCounter > 0) && (isGrounded || coyoteTimeCounter > 0))
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);  // Apply the initial jump force
            jumpBufferCounter = 0f;  // Reset the buffer once the jump is performed
            coyoteTimeCounter = 0f;  // Reset coyote time once the jump is performed
        }

        // If the player holds the jump button and the jump counter hasn't expired
        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, variableJumpForce);  // Apply additional force
                jumpTimeCounter -= Time.deltaTime;  // Decrease the counter
            }
            else
            {
                isJumping = false;
            }
        }

        // If the player releases the jump button, stop applying the upward force
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        // Apply fall multiplier to make the player fall faster when descending
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void ApplyJumpBuffer()
    {
        // Start the buffer counter if the jump button is pressed while in the air
        if (Input.GetButtonDown("Jump") && !isGrounded)
        {
            jumpBufferCounter = jumpBufferTime;
        }

        // Decrease the jump buffer counter over time
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    void ApplyCoyoteTime()
    {
        // Decrease the coyote time counter over time
        if (!isGrounded)
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        else
        {
            coyoteTimeCounter = coyoteTime;  // Reset the coyote time when the player is grounded
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
