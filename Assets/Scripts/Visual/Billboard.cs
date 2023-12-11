using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Player localPlayer;
    DetectiveAgencyPlayer det;
    public bool isDetectiveAgency;

    private void Start()
    {
        if(isDetectiveAgency)
        {
            det = GameObject.FindGameObjectWithTag("Player").GetComponent<DetectiveAgencyPlayer>();
        }
        else
        {
            localPlayer = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().currentPlayer;
        }
        
    }

    private void FixedUpdate()
    {
        if(localPlayer != null)
        {
            Vector3 directionToPlayer = transform.position - localPlayer.camTransform.position;
            directionToPlayer.y = 0;
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
        }
        else if(det != null)
        {
            Vector3 directionToPlayer = transform.position - det.camTransform.position;
            directionToPlayer.y = 0;
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
        }
    }
}
