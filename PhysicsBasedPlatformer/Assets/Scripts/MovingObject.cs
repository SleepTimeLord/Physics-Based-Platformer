using System.Collections;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public Vector3 startPosition = Vector3.zero;
    public Vector3 endPosition = Vector3.zero;
    private Vector3 targetPosition = Vector3.zero;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float waitTimeAtEndpoints = 1f;

    private bool isWaiting = false;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = endPosition;
    }

    void Update()
    {
        if (isWaiting) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            StartCoroutine(WaitAtEndpoint());
        }
    }

    private IEnumerator WaitAtEndpoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimeAtEndpoints);
        targetPosition = targetPosition == startPosition ? endPosition : startPosition;
        isWaiting = false;
    }
}