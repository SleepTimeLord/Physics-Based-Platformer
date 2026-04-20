using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    [Header("Input")]
    public InputAction playerMovement;

    [Header("Horizontal Movement")]
    [Tooltip("How much force is applied to start moving from 0.")]
    public float startAcceleration = 60f;
    [Tooltip("How much force is applied once already moving.")]
    public float runAcceleration = 35f;
    public float maxSpeed = 7f;
    [Tooltip("The force used to stop the character when no input is given.")]
    public float groundDeceleration = 40f;
    public float horizontalSpeed = 0f;

    [Header("Vertical Movement")]
    public float jumpForce = 14f;
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 2.5f;
    public float verticalSpeed = 0f;
    [Header("Animation")]
    private Animator animator;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    public bool isGrounded;
    public bool movingRight => moveInput.x > 0.01f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        playerMovement.Enable();
        playerMovement.performed += OnJumpPerformed;
    }

    void OnDisable()
    {
        playerMovement.Disable();
        playerMovement.performed -= OnJumpPerformed;
    }

    void Update()
    {
        moveInput = playerMovement.ReadValue<Vector2>();

        // Better Falling Physics
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.linearVelocity.y > 0 && !playerMovement.IsPressed())
        {
            rb.gravityScale = lowJumpMultiplier;
        }
        else
        {
            rb.gravityScale = 1f;
        }

        if (moveInput.x > 0.01f)
        {
            spriteRenderer.flipX = false; // facing right
        }
        else if (moveInput.x < -0.01f)
        {
            spriteRenderer.flipX = true; // facing left
        }
    }

    private void FixedUpdate()
    {
        ApplyRealisticMovement();
        verticalSpeed = rb.linearVelocity.y;
        horizontalSpeed = rb.linearVelocity.x;

        // animator parameters
        if (horizontalSpeed > 0.1f || horizontalSpeed < -0.1f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (verticalSpeed > 0.1f)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }
        else if (verticalSpeed < -0.1f)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        if (isGrounded)
        {
            animator.SetBool("isGrounded", true);
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }
    }

    private void ApplyRealisticMovement()
    {
        float currentAccel = (Mathf.Abs(rb.linearVelocity.x) < 0.1f) ? startAcceleration : runAcceleration;

        if (Mathf.Abs(moveInput.x) > 0.01f)
        {
            if (Mathf.Abs(rb.linearVelocity.x) < maxSpeed || Mathf.Sign(moveInput.x) != Mathf.Sign(rb.linearVelocity.x))
            {
                rb.AddForce(Vector2.right * moveInput.x * currentAccel, ForceMode2D.Force);
            }
        }
        else if (isGrounded)
        {
            float slowdown = Mathf.MoveTowards(rb.linearVelocity.x, 0, groundDeceleration * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector2(slowdown, rb.linearVelocity.y);
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (isGrounded && context.ReadValue<Vector2>().y > 0.5f)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Robot"))
        {
            isGrounded = true;
            transform.SetParent(collision.gameObject.transform, true);
        }

        if (collision.gameObject.CompareTag("Death"))
        {
            Debug.Log($"Collided with death object: {collision.gameObject.name}, resetting level.");
            GameManager.Instance.ResetLevel();
        }

        if (collision. gameObject.CompareTag("NextLevel"))
        {
            GameManager.Instance.NextLevel();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Robot"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Robot"))
        {
            isGrounded = false;
        }

        if (collision.gameObject.CompareTag("Robot"))
        {
            transform.SetParent(null);
        }
    }
}