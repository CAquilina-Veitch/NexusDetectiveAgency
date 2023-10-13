using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Nexus Detective Agency Components/ Holdable Object")]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public class Holdable : MonoBehaviour
{
    Collider cldr;
    Rigidbody rb;

    [HideInInspector] public Bounds cldrBounds;


    [TextArea]
    [Tooltip("Doesn't do anything. Just comments shown in inspector")]
    [SerializeField] string Note = "This component will need a collider to operate; can be any type.";

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
