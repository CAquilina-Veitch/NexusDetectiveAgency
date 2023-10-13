using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[AddComponentMenu("Nexus Detective Agency Components/ Pressable Button")]


public class Button : MonoBehaviour
{
    [Space(15)]
    public List<TriggerableObject> connectedTriggerables = new List<TriggerableObject>();

    [Space(15)]
    [Header("Delay before next press accepted.")]
    [SerializeField] float delayBetweenPresses;
    bool isBusy = false;

    [Space(20)]
    [Header("Required repaired cables to operate.")]
    public List<Repairable> requiredRepairs = new List<Repairable>();

    [Space(15)]
    [Header("Can be remotely activated")]
    [SerializeField] bool canBeRemotelyActivated;
    bool ownedRemotely;

    [Space(7)]
    public Sprite uiIcon;



    private void Awake()
    {
        foreach (TriggerableObject triggerable in connectedTriggerables)
        {
            if (!triggerable.inputSources.Contains(this))
            {
                triggerable.inputSources.Add(this);
            }

        }
    }

    bool isRepaired()
    {

        foreach(Repairable rpr in requiredRepairs)
        {
            if(!rpr.hasFuse)
            {
                return false;
            }
        }

        return true;
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
            GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().CollectedButtons.Add(this);
            
        }
        
        
        if(isRepaired())
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
