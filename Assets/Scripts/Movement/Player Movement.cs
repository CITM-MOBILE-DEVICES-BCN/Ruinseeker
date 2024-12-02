using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 10f;
    public float wallSlidingSpeed = 2f;
    public float wallJumpForce = 15f;
    public float wallCheckDistance = 0.5f;
    public float groundCheckDistance = 0.2f;
    public float leftRightGroundCheckOffset = 0.2f;
    public float wallCheckOffset = 0.2f;
    public float gravityScale = 1f;

    private bool facingRight = true;
    private bool isGrounded;
    private bool isWallSliding;
    private bool canJump = true;
    private enum PlayerState { Walking, Jumping, WallSliding, WallJumping }
    private PlayerState currentState;

    [Header("Components")]
    public Rigidbody2D rb;
    public BoxCollider2D playerCollider;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    private void Start()
    {
        currentState = PlayerState.Walking;
    }

    private void Update()
    {        
        HandleState();

        if (Input.GetMouseButtonDown(0) && canJump)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D GroundHitLeft = Physics2D.Raycast(transform.position + new Vector3(-leftRightGroundCheckOffset, 0, 0), Vector2.down, groundCheckDistance, groundLayer);
        RaycastHit2D GroundHitMiddle = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        RaycastHit2D GroundHitRight = Physics2D.Raycast(transform.position + new Vector3(leftRightGroundCheckOffset, 0, 0), Vector2.down, groundCheckDistance, groundLayer);

        isGrounded = GroundHitLeft.collider != null || GroundHitMiddle.collider != null || GroundHitRight.collider != null;

        RaycastHit2D WallHitTop = Physics2D.Raycast(transform.position + new Vector3(0, wallCheckOffset, 0), facingRight ? Vector2.right : Vector2.left, wallCheckDistance, wallLayer);
        RaycastHit2D WallHitMiddle = Physics2D.Raycast(transform.position, facingRight ? Vector2.right : Vector2.left, wallCheckDistance, wallLayer);
        RaycastHit2D WallHitBottom = Physics2D.Raycast(transform.position + new Vector3(0, -wallCheckOffset, 0), facingRight ? Vector2.right : Vector2.left, wallCheckDistance, wallLayer);

        isWallSliding = WallHitTop.collider != null || WallHitMiddle.collider != null || WallHitBottom.collider != null;

        if (!isGrounded && !isWallSliding)
        {
            rb.velocity += Vector2.down * gravityScale * Time.fixedDeltaTime;
        }

        if (isGrounded && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            canJump = true;
        }

        switch (currentState)
        {
            case PlayerState.Walking:
                Walk();
                break;
            case PlayerState.Jumping:
                JumpMovement();
                break;
            case PlayerState.WallSliding:
                WallSlide();
                break;
            case PlayerState.WallJumping:
                WallJump();
                break;
        }

        if (currentState != PlayerState.WallSliding)
        {
            currentState = PlayerState.Walking;
        }

        if (!isWallSliding && !isGrounded)
        {
            currentState = PlayerState.Jumping;
        }

    }

    private void HandleState()
    {
        switch (currentState)
        {
            case PlayerState.Walking:
                if (!isGrounded && isWallSliding)
                {
                    currentState = PlayerState.WallSliding;
                }
                break;

            case PlayerState.Jumping:
                if (rb.velocity.y <= 0)
                {
                    if (isWallSliding)
                    {
                        currentState = PlayerState.WallSliding;
                    }
                }
                break;

            case PlayerState.WallSliding:
                if (isGrounded)
                {
                    currentState = PlayerState.Walking;
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    currentState = PlayerState.WallJumping;
                    WallJump();
                }
                break;

            case PlayerState.WallJumping:
                if (rb.velocity.y <= 0)
                {
                    if (isGrounded)
                    {
                        currentState = PlayerState.Walking;
                    }
                    else if (isWallSliding)
                    {
                        currentState = PlayerState.WallSliding;
                    }
                }
                break;
        }
    }

    private void Walk()
    {
        float moveDirection = facingRight ? 1 : -1;
        rb.velocity = new Vector2(speed * moveDirection, rb.velocity.y);

        if (isGrounded && isWallSliding)
        {
            Flip();
        }
    }

    private void Jump()
    {
        if (currentState == PlayerState.Walking)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            currentState = PlayerState.Jumping;
            canJump = false;
        }
    }

    private void JumpMovement()
    {
        if (isGrounded)
        {
            currentState = PlayerState.Walking;
        }
    }

    private void WallSlide()
    {
        rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
    }

    private void WallJump()
    {
        Flip();
        float moveDirection = facingRight ? 1 : -1;
        rb.velocity = new Vector2(speed * moveDirection, wallJumpForce);
        currentState = PlayerState.Jumping;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(-leftRightGroundCheckOffset, 0, 0), transform.position + new Vector3(-leftRightGroundCheckOffset, 0, 0) + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(leftRightGroundCheckOffset, 0, 0), transform.position + new Vector3(leftRightGroundCheckOffset, 0, 0) + Vector3.down * groundCheckDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + new Vector3(0, wallCheckOffset, 0), transform.position + new Vector3(0, wallCheckOffset, 0) + (facingRight ? Vector3.right : Vector3.left) * wallCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + (facingRight ? Vector3.right : Vector3.left) * wallCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(0, -wallCheckOffset, 0), transform.position + new Vector3(0, -wallCheckOffset, 0) + (facingRight ? Vector3.right : Vector3.left) * wallCheckDistance);
    }
}
