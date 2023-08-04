using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CapsuleCollider cldr;
    public Camera cam;
    public Transform camTransform;

    public void SwitchTo(bool to)
    {
        cldr.enabled = to;
        cam.enabled = to;
    }

}
