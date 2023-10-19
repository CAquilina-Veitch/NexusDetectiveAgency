using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerInput : MonoBehaviour
{
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

    [Space(15)]
    [Header("Can be remotely activated")]
    [SerializeField] bool canBeRemotelyActivated;
    bool ownedRemotely;
    
    
    [Space(15)]
    [Header("Delay before next press accepted.")]
    [SerializeField] float delayBetweenPresses;
    bool isBusy = false;





    [Space(7)]
    public Sprite uiIcon;


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
            if (button.trigger==null)
            {
                button.trigger=this;
            }

        }


    }
    private void OnEnable()
    {
        isBusy = false;
    }
    public void Trigger()
    {
        if (canBeRemotelyActivated && !ownedRemotely)
        {
            ownedRemotely = true;
            GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().CollectedActions.Add(this);

        }


        if (isRepaired())
        {
            if (!isBusy)
            {
                isBusy = true;
                foreach (var trigger in connectedTriggerables)
                {
                    trigger.Triggered();
                }
                StartCoroutine(delay());
            }
        }

    }




    IEnumerator delay()
    {
        yield return delayBetweenPresses;
        isBusy = false;
    }
}
