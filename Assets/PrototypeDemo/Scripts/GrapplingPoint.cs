using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingPoint : Interactable
{
    // Start is called before the first frame update
    public Vector3 position;
    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Interact()
    {
        Debug.Log("Interacted with" + gameObject.name);
        if (position != Vector3.zero)
        {
            player.transform.position = position;
        }
    }
}
