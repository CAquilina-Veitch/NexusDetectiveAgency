using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorExit : MonoBehaviour
{
    [SerializeField] UnityEvent Exit;

private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Exit.Invoke();
        }
    }
}
