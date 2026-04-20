using UnityEngine;

public class PressableButton : MonoBehaviour
{
    [Header("Press Settings")]
    public float pressDepth = 1f;
    public float pressSpeed = 10f;
    public float releaseSpeed = 6f;

    [Header("References")]
    public Transform buttonSprite;
    [SerializeField]
    private MonoBehaviour linkedDoorObject;
    private IOpenable linkedDoor => linkedDoorObject as IOpenable;

    private Vector3 restPosition;
    private Vector3 pressedPosition;
    private int contactCount = 0;
    private bool wasPressed = false;

    void Start()
    {
        restPosition = buttonSprite.localPosition;
        pressedPosition = restPosition + Vector3.down * pressDepth;
    }

    void Update()
    {
        Vector3 target = contactCount > 0 ? pressedPosition : restPosition;
        float speed = contactCount > 0 ? pressSpeed : releaseSpeed;

        buttonSprite.localPosition = Vector3.Lerp(
            buttonSprite.localPosition,
            target,
            Time.deltaTime * speed
        );

        // Snap when close enough to avoid asymptotic jitter
        if (Vector3.Distance(buttonSprite.localPosition, target) < 0.01f)
            buttonSprite.localPosition = target;

        bool fullyPressed = contactCount > 0 && buttonSprite.localPosition == pressedPosition;

        if (fullyPressed && !wasPressed)
        {
            linkedDoor.Open();
            wasPressed = true;
        }
        else if (!fullyPressed && wasPressed)
        {
            linkedDoor.Close();
            wasPressed = false;
        }
    }

    public bool IsFullyPressed()
    {
        return buttonSprite.localPosition == pressedPosition;
    }

    public void OnContact(bool entered)
    {
        contactCount += entered ? 1 : -1;
        contactCount = Mathf.Max(0, contactCount);
    }
}