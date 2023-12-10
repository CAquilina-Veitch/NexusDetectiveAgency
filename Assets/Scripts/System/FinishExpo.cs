using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishExpo : MonoBehaviour
{
    [SerializeField] UnityEvent FinishedExposition;
    public void FinEx()
    {
        FinishedExposition.Invoke();
    }
}
