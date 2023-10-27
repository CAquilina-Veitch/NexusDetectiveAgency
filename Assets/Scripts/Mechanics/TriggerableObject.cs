using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Nexus Detective Agency Components/ Triggers/ Affectable Object")]

[Serializable]
public enum InputMode
{
    Permanent, Trigger, Toggle
}

public class TriggerableObject : MonoBehaviour
{

    [TextArea(1, 20)]
    [Tooltip("Doesn't do anything. Just comments shown in inspector")]
    public string Note = "if the mode is set to pulse, the button will do the on action every press, then off after pulselength, but toggle will do on then off then on etc. \r\n\r\nPressure plates use toggle, with leaving the plate being the off,  pulselength is ";




    [Space(15)]
    [Header("Inputs.")]

    public List<Button> buttons = new List<Button>();
    public List<PressurePlate> pressurePlates = new List<PressurePlate>();
    public InputMode mode;


    [Space(20)]
    [Header("Required repaired cables to operate.")]
    public List<Repairable> requiredRepairs = new List<Repairable>();






    bool currentlyPowered;

    bool currentlyOutputting;





    public float pulseLength;

    
    public UnityEvent triggerOnActions;
    public UnityEvent triggerOffActions;


    [Space(15)]
    [Header("Can be remotely activated")]
    [SerializeField] bool canBeRemotelyActivated;
    bool ownedRemotely;


    [Space(7)]
    public Sprite uiIcon;


    private void Awake()
    {


        foreach (Button button in buttons)
        {
            if (button.trigger == null)
            {
                button.trigger = this;
            }

        }
        foreach (Repairable rep in requiredRepairs)
        {
            rep.trigger = this;
        }


    }


    public void Triggered()
    {
        TryAddRemoteControl();

        if (mode == InputMode.Permanent)
        {
            if (!currentlyPowered)
            {
                StartCoroutine(TriggerWithDelay());
            }
        }
        else if (mode == InputMode.Trigger)
        {
            if(IsRepaired())
            {
                if (!currentlyPowered)
                {
                    Turn(true);
                }
            }

        }
        else if (mode == InputMode.Toggle) 
        {
            if(currentlyPowered)
            {
                //toggle off
                Turn(false);
            }
            else
            {
                //check repairs
                if(IsRepaired())
                {
                    //toggle on
                    Turn(true);
                }

            }
        }
    }


    bool IsRepaired()
    {
        foreach (Repairable rpr in requiredRepairs)
        {
            if (!rpr.hasFuse)
            {
                return false;
            }
        }

        return true;
    }

    public void UpdateRepairs()
    {
        if (!IsRepaired())
        {
            Turn(false);
        }
    }




    IEnumerator TriggerWithDelay()
    {
        Turn(true);
        yield return new WaitForSeconds(pulseLength);
        if(currentlyOutputting)
        {
            Turn(false);
        }

    }

    public void Turn(bool to)
    {
        currentlyPowered = to;

        if (to)
        {
            if (currentlyOutputting)
            {
                Debug.LogWarning("Already on");
            }
            else
            {
                //check if can
                if (IsRepaired())
                {
                    currentlyOutputting = true;
                    triggerOnActions.Invoke();
                }
            }
        }
        else
        {
            if (currentlyOutputting)
            {
                currentlyOutputting = false;
                triggerOffActions.Invoke();
            }
            else
            {
                Debug.LogWarning("Already Off");
            }
        }

    }




    void TryAddRemoteControl()
    {
        if (canBeRemotelyActivated && !ownedRemotely)
        {
            ownedRemotely = true;
            Debug.Log(GameObject.FindGameObjectWithTag("PlayerController"));
            Debug.Log(GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>());
            Debug.Log(GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().CollectedActions);
            GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().CollectedActions.Add(this);

        }
    }

}
