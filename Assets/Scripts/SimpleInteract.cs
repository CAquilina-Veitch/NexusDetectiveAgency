using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleInteract : MonoBehaviour
{
    public UnityEvent LoreInteract;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            LoreInteract.Invoke();
        }
    }
}
