using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform targetPos;
    public float speed = 1f;
    public GameObject thisCam;

    //public CameraSwitch cameraSwitch;

    //private void OnEnable()
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, targetPos.position, speed * Time.deltaTime);
    //}

    private void Update()
    {
        if (thisCam.activeSelf == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, speed * Time.deltaTime);
        }
    }
}
