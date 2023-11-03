using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawning : MonoBehaviour
{
    Vector3 spawnPosition;
    Quaternion spawnRotation;
    private void Start()
    {
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    public void Respawn()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
    }

}
