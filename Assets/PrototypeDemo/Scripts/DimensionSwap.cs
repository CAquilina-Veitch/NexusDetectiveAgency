using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DimensionSwap : Interactable
{
    // Start is called before the first frame update
    [SerializeField] UnityEvent Activate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Interact()
    {
        Activate.Invoke();
    }
}
