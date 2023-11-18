using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePlatform : MonoBehaviour
{

    [SerializeField] Transform playerCam;
    [SerializeField] PlayerController playerControls;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float verticalHeight;

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, groundLayerMask))
        {
            Debug.DrawRay(playerCam.transform.position, playerCam.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            transform.position = hit.point;
        }
        else
        {
            Debug.DrawRay(playerCam.transform.position, playerCam.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }


        transform.position = playerCam.transform.position + playerCam.transform.forward * playerControls.dronePlatformDistance;
    }




}
