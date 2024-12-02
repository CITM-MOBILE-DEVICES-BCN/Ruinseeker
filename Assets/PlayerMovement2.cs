using UnityEngine;
using System.Collections;
public class PlayerMovement2 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 10f;
    public float wallSlidingSpeed = 2f;
    public float wallJumpForce = 15f;
    public float wallCheckDistance = 0.5f;

    private bool facingRight = true;
    private bool isGrounded;
    private bool isWallSliding;
    private bool canJump = true;

    [Header("Components")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    private enum PlayerState { Walking, Jumping, WallSliding, WallJumping }
    private PlayerState currentState;

    void Start()
    {
        currentState = PlayerState.Walking; // El jugador comienza caminando
    }

    void Update()
    {
        // Verificar si el jugador está tocando el suelo y las paredes usando colisiones
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        isWallSliding = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);

        // Llamada a la función de la máquina de estados
        HandleState();

        // Detectar el click o tap para el salto
        if (Input.GetMouseButtonDown(0)) // Mouse o toque en pantalla
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Actualizar el movimiento según el estado actual
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
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case PlayerState.Walking:
                if (isGrounded)
                {
                    if (isWallSliding) // Si está en la pared y tocando el suelo
                    {
                        currentState = PlayerState.WallSliding;
                    }
                }
                else if (isWallSliding) // Si está deslizándose por la pared
                {
                    currentState = PlayerState.WallSliding;
                }
                break;

            case PlayerState.Jumping:
                if (rb.velocity.y <= 0) // Cuando el jugador cae
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
                else if (Input.GetMouseButtonDown(0) && canJump) // Salto desde la pared
                {
                    currentState = PlayerState.WallJumping;
                    canJump = false;
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
        // Movimiento del jugador en la dirección que está mirando
        float moveDirection = facingRight ? 1 : -1;
        rb.velocity = new Vector2(speed * moveDirection, rb.velocity.y);

        // Cambiar la dirección cuando el jugador toca la pared mientras camina
        if (isGrounded && isWallSliding)
        {
            Flip();
        }
    }

    private void Jump()
    {
        if (currentState == PlayerState.Walking || currentState == PlayerState.WallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);  // Aplica salto
            currentState = PlayerState.Jumping;
            canJump = false;  // Impide el salto múltiple
        }
    }

    private void JumpMovement()
    {
        // Si el jugador no está saltando, vuelve a caminar si toca el suelo
        if (isGrounded)
        {
            currentState = PlayerState.Walking;
        }
    }

    private void WallSlide()
    {
        // Movimiento de deslizamiento por la pared
        rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
    }

    private void WallJump()
    {
        // Primero, el salto vertical (hacia arriba)
        rb.velocity = new Vector2(rb.velocity.x, wallJumpForce);

        // Ahora espera un pequeño intervalo para cambiar la dirección y saltar hacia la pared opuesta
        Invoke("ApplyWallJump", 0.1f);  // Retardo para el cambio de dirección
    }

    private void ApplyWallJump()
    {
        // Realizar el flip para cambiar la dirección
        Flip();

        // Ahora empujar al jugador hacia la dirección opuesta de la pared en diagonal
        float wallJumpDirection = facingRight ? -1 : 1;
        rb.velocity = new Vector2(wallJumpDirection * wallJumpForce, rb.velocity.y);  // Movimiento diagonal
    }

    void Dash()
    {
        if (facingRight)
        {
            rb.AddForce(Vector2.right * 20f, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.left * 20f, ForceMode2D.Impulse);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheck.position, 0.2f);
    }
}
