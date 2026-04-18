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
    public float fallMultiplier = 3f; // For a heavier, realistic fall
    public float lowJumpMultiplier = 2.5f; // For variable jump height
    public float verticalSpeed = 0f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public bool isGrounded;
    public bool movingRight => moveInput.x > 0.01f;
    public bool isFacingRight => transform.localScale.x > 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Realistic physics tip: Ensure 'Interpolate' is on in the Rigidbody2D inspector!
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

        // make the character face the right way
        if (movingRight)
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); 
        }
        else if (moveInput.x < -0.01f)
        {
            transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f); 
        }
    }

    private void FixedUpdate()
    {
        ApplyRealisticMovement();
        verticalSpeed = rb.linearVelocity.y;
        horizontalSpeed = rb.linearVelocity.x;
    }

    private void ApplyRealisticMovement()
    {
        // Determine which acceleration to use 
        float currentAccel = (Mathf.Abs(rb.linearVelocity.x) < 0.1f) ? startAcceleration : runAcceleration;

        // Apply Horizontal Force
        if (Mathf.Abs(moveInput.x) > 0.01f)
        {
            // Only add force if we are below maxSpeed or trying to turn around
            if (Mathf.Abs(rb.linearVelocity.x) < maxSpeed || Mathf.Sign(moveInput.x) != Mathf.Sign(rb.linearVelocity.x))
            {
                rb.AddForce(Vector2.right * moveInput.x * currentAccel, ForceMode2D.Force);
            }
        }
        // Friction
        else if (isGrounded)
        {
            // Apply a counter-force to simulate feet gripping the ground
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Using Stay is safer for "Surface" tags to ensure isGrounded remains true while standing
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