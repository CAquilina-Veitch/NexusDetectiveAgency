using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Player localPlayer;

    private void OnEnable()
    {
        localPlayer = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().currentPlayer;
    }

    private void FixedUpdate()
    {
        if(localPlayer != null)
        {
            Vector3 directionToPlayer = transform.position - localPlayer.camTransform.position;
            directionToPlayer.y = 0;
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
        }
    }
}
