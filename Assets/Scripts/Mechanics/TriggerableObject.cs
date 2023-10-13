using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerableObject : MonoBehaviour
{
    public List<Button> inputSources = new List<Button>();

    public UnityEvent onTriggerActions;


    private void Awake()
    {
        foreach(Button button in inputSources)
        {
            if (!button.connectedTriggerables.Contains(this))
            {
                button.connectedTriggerables.Add(this);
            }
            
        }
    }

    public void Triggered()
    {
        onTriggerActions.Invoke();
    }


}
