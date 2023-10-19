using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum InputType { Pulse, Toggle }


public class TriggerableObject : MonoBehaviour
{
    public TriggerInput trigger;


    [Header("if the mode is set to pulse, the button will do the on action every press, then off after pulselength, but toggle will do on then off then on etc. Pressure plates use toggle, with leaving the plate being the off,  pulselength is ")]
    public InputType inputType;
    public float pulseLength;

    
    public UnityEvent triggerOnActions;
    public UnityEvent triggerOffActions;

    bool state;



    public void Triggered()
    {
        if (!state)
        {
            state = true;
            if(inputType == InputType.Pulse)
            {
                StartCoroutine(Delay());
            }
            else
            {
                triggerOnActions.Invoke();
            }

        }
        else
        {
            state = false;
            if (inputType == InputType.Toggle)
            {
                triggerOffActions.Invoke();
            }
            else
            {

            }
        }



        

    }


    IEnumerator Delay()
    {
        triggerOnActions.Invoke();
        yield return new WaitForSeconds(pulseLength);
        if (state)
        {
            state = false;
            triggerOffActions.Invoke();
        }
        else
        {
            triggerOnActions.Invoke();
        }

    }

}
