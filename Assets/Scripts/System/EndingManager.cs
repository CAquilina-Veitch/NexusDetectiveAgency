using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Linq.Expressions;

public class EndingManager : MonoBehaviour
{
    [SerializeField] UnityEvent EndingOne;
    [SerializeField] UnityEvent EndingTwo;
    [SerializeField] UnityEvent EndingThree;

    private void Start()
    {
        int end = GameObject.FindGameObjectWithTag("EndingCarrier").GetComponent<EndingCarrier>().endingChosen;
        switch (end)
        {
            case 1:
                EndingOne.Invoke();
                break;
            case 2:
                EndingTwo.Invoke();
                break;
            case 3:
                EndingThree.Invoke();
                break;
            default:
                Debug.LogError("THIS SHUD NOT BE " + end);
                break;
        }

    }


}
