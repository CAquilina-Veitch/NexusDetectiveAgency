using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicInteract : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] UnityEvent Interact;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Interact.Invoke();
        }
    }
}
