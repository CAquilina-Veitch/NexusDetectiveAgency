using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[AddComponentMenu("Nexus Detective Agency Components/ Triggers/ Trigger Input Manager")]


public class TriggerInput : MonoBehaviour
{
    [TextArea(1, 20)]
    [Tooltip("Doesn't do anything. Just comments shown in inspector")]
    public string Note = "This component is needed for a button or pressure plate to interface with a triggerable object. \r\nThis component contains those inputs, and is in charge of checking for repairs, and allowing remote activation of the connected actions. \r\n\r\nCan be attached to anything, but each trigger input component should be a unique set of actions";



    [Space(15)]
    [Header("Inputs.")]

    public List<Button> buttons = new List<Button>();
    public List<PressurePlate> pressurePlates = new List<PressurePlate>();



    [Space(5)]
    [Header("ObjectsToTriggerAction.")]
    public List<TriggerableObject> connectedTriggerables = new List<TriggerableObject>();


    [Space(20)]
    [Header("Required repaired cables to operate.")]
    public List<Repairable> requiredRepairs = new List<Repairable>();

    
    



    bool currentlyPowered;



    [Space(7)]
    public Sprite uiIcon;

    public void updateRepairs()
    {
        if (currentlyPowered)
        {
            if (!isRepaired())
            {
                foreach (TriggerableObject triggerable in connectedTriggerables)
                {
                    triggerable.UnRepair();
                }
            }
        }
    }
    bool isRepaired()
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



    private void Awake()
    {


        foreach (Button button in buttons)
        {
            if (button.trigger == null) 
            {
                button.trigger = this;
            }

        }
        foreach(Repairable rep in requiredRepairs)
        {
            rep.trigger = this;
        }


    }


    public void Toggle()
    {
        if (isRepaired())
        {
            foreach (var trigger in connectedTriggerables)
            {
                trigger.Toggled(currentlyPowered);
            }
        }
    }    
    public void Toggle(bool to)
    {
        if (to)
        {
            if (isRepaired())
            {
                currentlyPowered = true;
                foreach (var trigger in connectedTriggerables)
                {
                    trigger.Toggled(currentlyPowered);
                }
            }
        }
        else
        {
            currentlyPowered = false;
            foreach (var trigger in connectedTriggerables)
            {
                trigger.Toggled(currentlyPowered);
            }
        }
    }


    public void Trigger()
    {
        if (isRepaired())
        {
            foreach (var trigger in connectedTriggerables)
            {
                trigger.Triggered();
            }

        }

    }

}
