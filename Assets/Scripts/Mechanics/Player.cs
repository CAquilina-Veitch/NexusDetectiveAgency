using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CapsuleCollider feetCollider;
    public CapsuleCollider bodyCollider;
    public Camera cam;
    public Camera handCam;
    public Transform camTransform;
    public Animator anim;

    public void SwitchTo(bool to)
    {
        feetCollider.enabled = to;
        bodyCollider.enabled = to;
        cam.enabled = to;
        handCam.enabled = to;
    }

}
