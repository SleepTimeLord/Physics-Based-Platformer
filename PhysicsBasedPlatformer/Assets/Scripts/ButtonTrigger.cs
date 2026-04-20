using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    private PressableButton parentButton;

    void Awake()
    {
        parentButton = GetComponentInParent<PressableButton>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Robot"))
            parentButton.OnContact(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Robot"))
            parentButton.OnContact(false);
    }
}
