using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]

public class Holdable : MonoBehaviour
{
    Collider cldr;
    Rigidbody rb;

    public Bounds cldrBounds;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cldr = GetComponent<Collider>();
        cldrBounds = cldr.bounds;
    }
    public void ToggleGhost(bool to)
    {
        cldr.enabled = !to;
        rb.useGravity = !to;
        rb.isKinematic = to;
    }
}
