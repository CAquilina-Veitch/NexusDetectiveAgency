using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleColliderEvent : MonoBehaviour
{
    [SerializeField] UnityEvent ColliderEnter;
    [SerializeField] UnityEvent ColliderExit;

    // Update is called once per frame
    private void OnTriggerEnter(Collider collision)
    {
        ColliderEnter.Invoke();
    }

    private void OnTriggerExit(Collider collision)
    {
        ColliderExit.Invoke();
    }

}
