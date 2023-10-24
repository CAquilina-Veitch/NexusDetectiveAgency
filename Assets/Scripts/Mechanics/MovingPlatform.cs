using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Nexus Detective Agency Components/ Moving Platform")]

public class MovingPlatform : MonoBehaviour
{
    public Transform movingPlatform;   // The child platform to move
    public List<Transform> pathPoints;  // List of points for the platform to move between
    public float movementSpeed = 2.0f; // Set the movement speed

    private int currentPointIndex = 0;
    private bool isMoving = false;
    private Coroutine movementCoroutine;

    private void Start()
    {
        // Ensure the platform is at the initial point
        if (movingPlatform != null && pathPoints.Count > 0)
        {
            movingPlatform.position = pathPoints[currentPointIndex].position;
        }
        StartMovingPlatform();
    }

    public void StartMovingPlatform()
    {
        if (movingPlatform != null && pathPoints.Count > 0 && !isMoving)
        {
            movementCoroutine = StartCoroutine(MovePlatform());
        }
    }

    public void StopMovingPlatform()
    {
        if (isMoving && movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            isMoving = false;
        }
    }

    IEnumerator MovePlatform()
    {
        movingPlatform.position = pathPoints[0].position;
        isMoving = true;
        while (currentPointIndex < pathPoints.Count)
        {
            Vector3 targetPosition = pathPoints[currentPointIndex].position;
            while (Vector3.Distance(movingPlatform.position, targetPosition) > 0.01f)
            {
                movingPlatform.position = Vector3.MoveTowards(movingPlatform.position, targetPosition, movementSpeed * Time.deltaTime);
                yield return null;
            }

            // Move to the next point
            currentPointIndex++;

            yield return null;
        }

        isMoving = false;
        movingPlatform.position = pathPoints[pathPoints.Count-1].position;
    }
}
