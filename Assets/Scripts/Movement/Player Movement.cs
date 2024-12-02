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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;

        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, facingRight ? Vector2.right : Vector2.left, wallCheckDistance, wallLayer);
        isWallSliding = hit2.collider != null;

        // Aplicar gravedad solo si no est�s en el suelo o desliz�ndote por la pared
        if (!isGrounded && !isWallSliding)
        {
            rb.velocity += Vector2.down * gravityScale * Time.fixedDeltaTime;
        }

        // Si est� tocando el suelo y la velocidad en Y es negativa, detener el movimiento en Y
        if (isGrounded && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            canJump = true;  // Permitir el salto solo cuando haya tocado el suelo
        }

        // Manejo de los estados seg�n la situaci�n
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

        // Si no hay pared y no est� en el suelo, el jugador debe caer
        if (!isWallSliding && !isGrounded)
        {
            currentState = PlayerState.Jumping;  // Deber�as estar en estado de salto o ca�da
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
                    else if (isGrounded)
                    {
                        currentState = PlayerState.Walking;
                    }
                }
                break;

            case PlayerState.WallSliding:
                if (isGrounded)
                {
                    currentState = PlayerState.Walking;
                }
                else if (Input.GetMouseButtonDown(0)) // Detectar clic para iniciar el salto
                {
                    currentState = PlayerState.WallJumping;
                    WallJump(); // Ejecutar directamente el salto desde la pared
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
        if (currentState == PlayerState.Walking || currentState == PlayerState.WallSliding)
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
        float wallJumpDirection = facingRight ? -1 : 1;
        rb.velocity = new Vector2(wallJumpDirection * speed, wallJumpForce);

        Invoke(nameof(Flip), 0.1f);
    }

    private void EndWallJump()
    {
        currentState = PlayerState.Jumping;
        canJump = false;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + (facingRight ? Vector3.right : Vector3.left) * wallCheckDistance);
    }
}
