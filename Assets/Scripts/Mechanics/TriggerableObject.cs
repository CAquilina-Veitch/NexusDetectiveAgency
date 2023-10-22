using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Nexus Detective Agency Components/ Triggers/ Affectable Object")]

public class TriggerableObject : MonoBehaviour
{
    public TriggerInput trigger;

    [TextArea(1, 20)]
    [Tooltip("Doesn't do anything. Just comments shown in inspector")]
    public string Note = "if the mode is set to pulse, the button will do the on action every press, then off after pulselength, but toggle will do on then off then on etc. \r\n\r\nPressure plates use toggle, with leaving the plate being the off,  pulselength is ";
    
    
    public float pulseLength;

    
    public UnityEvent triggerOnActions;
    public UnityEvent triggerOffActions;

    bool state;


    [Space(15)]
    [Header("Can be remotely activated")]
    [SerializeField] bool canBeRemotelyActivated;
    bool ownedRemotely;




    public void Triggered()
    {
        TryAddRemoteControl();

        if (!state)
        {
            state = true;
            StartCoroutine(Delay());

        }
        else
        {
            state = false;
        }

    }

    public void Toggled(bool to)
    {
        if(state)
        {
            if (to)
            {
                Debug.LogError("On to on");
            }
            else
            {
                triggerOffActions.Invoke();
                state = false;
                Debug.Log(61);
            }
        }
        else
        {
            if (to)
            {
                StartCoroutine(ToggleDelay(true));
                Debug.Log(62);
            }
            else
            {
                
                Debug.LogError("Off to off");
            }
        }
    }

    IEnumerator ToggleDelay(bool toOn)
    {
        state = toOn;
        triggerOnActions.Invoke();
        yield return new WaitForSeconds(pulseLength);
        if(!state)
        {
            triggerOffActions.Invoke();
        }
    }



    public void UnRepair()
    {
        if (state)
        {
            state = false;
            triggerOffActions.Invoke();
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
            StartCoroutine(Delay());
        }

    }






    void TryAddRemoteControl()
    {
        if (canBeRemotelyActivated && !ownedRemotely)
        {
            ownedRemotely = true;
            GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().CollectedActions.Add(this);

        }
    }

}
