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
        if (collision.tag == "Player")
        {
            ColliderEnter.Invoke();

        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            ColliderExit.Invoke();
        }
    }

}
