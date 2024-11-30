using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 10f;
    public float dashForce = 20f;
    private bool facingRight = true;
    private bool isGrounded;
    private bool isWallSliding;
    private bool canJump = true;
    private float wallSlidingSpeed = 2f;

    [Header("Touch Settings")]
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    public float swipeThreshold = 50f;

    [Header("Components")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (Input.GetMouseButtonDown(0) && canJump && (isGrounded || isWallSliding))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canJump = false;
        }

        if (isGrounded)
        {
            canJump = true;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    break;
                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    CheckSwipe();
                    break;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            CheckSwipe();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * (facingRight ? 1 : -1), rb.velocity.y);

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (isGrounded)
            {
                isWallSliding = false;
                CancelInvoke("StartSliding");
                Flip();
            }
            else if (!isGrounded)
            {
                isWallSliding = true;
                Invoke("StartSliding", 0.5f);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || isGrounded)
        {
            isWallSliding = false;
            CancelInvoke("StartSliding");
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void StartSliding()
    {
        rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
    }

    void CheckSwipe()
    {
        float swipeDistance = Vector2.Distance(startTouchPosition, endTouchPosition);
        if (swipeDistance > swipeThreshold)
        {
            Vector2 swipeDirection = (endTouchPosition - startTouchPosition).normalized;
            if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
            {
                if (swipeDirection.x > 0)
                {
                    rb.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
                    if (!facingRight)
                        Flip();
                }
                else
                {
                    rb.AddForce(Vector2.left * dashForce, ForceMode2D.Impulse);
                    if (facingRight)
                        Flip();
                }

                StartCoroutine(ChangeDirectionOnLanding());
            }
        }
    }

    IEnumerator ChangeDirectionOnLanding()
    {
        yield return new WaitForSeconds(0.2f);

        if (isGrounded)
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }
}