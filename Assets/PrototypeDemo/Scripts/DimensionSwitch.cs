using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DimensionSwitch : MonoBehaviour
{
    [SerializeField] UnityEvent InteractWith;
    // Start is called before the first frame update
    private void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            InteractWith.Invoke();
        }
    }
}