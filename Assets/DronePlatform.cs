using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePlatform : MonoBehaviour
{

    [SerializeField] Transform playerCam;
    [SerializeField] PlayerController playerControls;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float verticalHeight;

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            hit.point = transform.position;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }


        transform.position = playerCam.transform.position + playerCam.transform.forward * playerControls.dronePlatformDistance;
    }




}
