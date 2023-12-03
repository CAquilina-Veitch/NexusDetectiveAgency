using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

[AddComponentMenu("Nexus Detective Agency Components/ Moving Platform")]

public class MovingPlatform : MonoBehaviour
{
    public Transform movingPlatform;   // The child platform to move
    public List<Transform> pathPoints;  // List of points for the platform to move between
    public float movementSpeed = 2.0f; // Set the movement speed

    bool forward = false;

    private float currentPointIndex;



    private void FixedUpdate()
    {
        if(currentPointIndex > 0 && currentPointIndex < pathPoints.Count-1)
        { 

            currentPointIndex += Time.deltaTime * movementSpeed * (forward ? 1 : -1);
            currentPointIndex = Mathf.Clamp(currentPointIndex, 0, pathPoints.Count-1);
            float i = currentPointIndex % 1;
            movingPlatform.position = Vector3.Lerp(pathPoints[((int)Mathf.Floor(i))].position, pathPoints[(int)Mathf.Ceil(i)].position, i);
            if (i == 0)
            {
                movingPlatform.position = pathPoints[Mathf.RoundToInt(currentPointIndex)].position;
            }
            Debug.Log($"{currentPointIndex} , {pathPoints[((int)Mathf.Floor(i))].position}, {pathPoints[(int)Mathf.Ceil(i)].position} , {i}");
        }
    }
    private void OnEnable()
    {
        movingPlatform.position = pathPoints[Mathf.RoundToInt(currentPointIndex)].position;
    }

    public void DirectionFlip()
    {
        Debug.Log("AA");
        forward = !forward;
        currentPointIndex += (forward ? 0.01f : -0.01f);
    }



}
