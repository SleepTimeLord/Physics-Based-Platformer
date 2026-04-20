using UnityEngine;

public class Door : MonoBehaviour, IOpenable
{
    [Header("Opening Settings")]
    public float pressDepth = 1f;
    public float pressSpeed = 10f;
    public float releaseSpeed = 6f;

    [Header("References")]
    public Transform doorSprite;
    private Vector3 restPosition;
    private Vector3 openPosition;
    private bool isOpen = false; // ← state flag

    void Start()
    {
        restPosition = doorSprite.localPosition;
        openPosition = restPosition + Vector3.up * pressDepth;
    }

    void Update()
    {
        // runs every frame, smoothly moves door toward target
        Vector3 target = isOpen ? openPosition : restPosition;
        float speed = isOpen ? pressSpeed : releaseSpeed;

        doorSprite.localPosition = Vector3.Lerp(
            doorSprite.localPosition,
            target,
            Time.deltaTime * speed
        );
    }

    public void Open()  { isOpen = true; }
    public void Close() { isOpen = false; }
}
