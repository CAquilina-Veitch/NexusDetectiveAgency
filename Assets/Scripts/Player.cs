using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CapsuleCollider feetCollider;
    public CapsuleCollider bodyCollider;
    public Camera cam;
    public Transform camTransform;

    public void SwitchTo(bool to)
    {
        feetCollider.enabled = to;
        bodyCollider.enabled = to;
        cam.enabled = to;
    }

}
