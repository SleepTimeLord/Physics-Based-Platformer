using UnityEngine;
using UnityEngine.InputSystem;

public enum RobotState
{
    Idle,
    MovingForward,
    MovingBackward,
    MovingUp,
    MovingDown,
    Debug
}

public class RobotController : MonoBehaviour
{
    private Transform robotTransform;

    public LayerMask collidingLayer;
    [Header("Movement Settings")]
    public InputAction inputAction;
    public float forwardSpeed = 5f;
    public float backwardSpeed = 5f;
    public float verticalSpeed = 5f;
    public float raycastDistanceY = 1f;
    public float raycastDistanceX = 1f;
    public bool debugMode = false;
    public Vector3 intialPosition;

    

    [HideInInspector]
    public RobotState currentState;

    private void Start()
    {
        Vector3 initialPos = transform.position;
        robotTransform = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    private void Update()
    {
        if (debugMode)
        {
            ManualMovement(); // sets state AND moves
        }
        CastRay();

        switch (currentState)
        {
            case RobotState.Debug:
                // movement 
                ManualMovement();
                break;
            case RobotState.MovingForward:
                MoveForward();
                break;
            case RobotState.MovingBackward:
                MoveBackward();
                break;
            case RobotState.MovingUp:
                MoveUp();
                break;
            case RobotState.MovingDown:
                MoveDown();
                break;
            case RobotState.Idle:
            default:
                Stop();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Surface"))
        {
            print("Collided with ground/surface, stopping robot.");
            currentState = RobotState.Idle;
        }

        if (other.gameObject.CompareTag("Death"))
        {
            transform.position = intialPosition;
        }
    }

    public void MoveForward()
    {
        robotTransform.Translate(Vector3.right * Time.deltaTime * forwardSpeed);
    }

    public void MoveBackward()
    {
        robotTransform.Translate(Vector3.left * Time.deltaTime * backwardSpeed);
    }

    public void MoveUp()
    {
        robotTransform.Translate(Vector3.up * Time.deltaTime * verticalSpeed);
    }

    public void MoveDown()
    {
        robotTransform.Translate(Vector3.down * Time.deltaTime * verticalSpeed);
    }

    public void Stop()
    {
        // no movement
    }

    public void ManualMovement()
    {
        Vector2 moveInput = inputAction.ReadValue<Vector2>();

        if (moveInput.x > 0.01f)
        {
            currentState = RobotState.MovingForward;
            MoveForward();
        }
        else if (moveInput.x < -0.01f)
        {
            currentState = RobotState.MovingBackward;
            MoveBackward();
        }
        else if (moveInput.y > 0.01f)
        {
            currentState = RobotState.MovingUp;
            MoveUp();
        }
        else if (moveInput.y < -0.01f)
        {
            currentState = RobotState.MovingDown;
            MoveDown();
        }
        else
        {
            currentState = RobotState.Idle;
            Stop();
        }
    }

    // call this from SpeechManager for voice commands
    public void SetState(RobotState newState)
    {
        currentState = newState;
    }

    // Casts a box in the direction of movement to check for obstacles
    public void CastRay()
    {
        float raycastDistance;
        Vector2 direction = Vector2.zero;
        Vector2 boxSize;
        Vector2 fullSize = new Vector2(
        GetComponent<BoxCollider2D>().size.x * robotTransform.lossyScale.x,
        GetComponent<BoxCollider2D>().size.y * robotTransform.lossyScale.y
    );

        switch (currentState)
        {
            case RobotState.MovingForward:
                direction = Vector2.right;
                raycastDistance = raycastDistanceX;
                boxSize = new Vector2(.05f, fullSize.y);
                break;
            case RobotState.MovingBackward:
                direction = Vector2.left;
                raycastDistance = raycastDistanceX;
                boxSize = new Vector2(.05f, fullSize.y);
                break;
            case RobotState.MovingUp:
                direction = Vector2.up;
                raycastDistance = raycastDistanceY;
                boxSize = new Vector2(fullSize.x, 0.05f);
                break;
            case RobotState.MovingDown:
                direction = Vector2.down;
                raycastDistance = raycastDistanceY;
                boxSize = new Vector2(fullSize.x, 0.05f);
                break;
            default: return;
        }

        RaycastHit2D hit = Physics2D.BoxCast(robotTransform.position, boxSize, 0f, direction, raycastDistance, collidingLayer);

        if (hit.collider != null)
        {
            currentState = RobotState.Idle;
        }
    }

}