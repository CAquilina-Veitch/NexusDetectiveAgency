using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicInteract : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] UnityEvent Interact;
    [SerializeField] UnityEvent SecondaryInteract;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Interact.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SecondaryInteract.Invoke();
        }
    }
}
