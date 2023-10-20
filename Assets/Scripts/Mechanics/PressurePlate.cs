using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Nexus Detective Agency Components/ Triggers/ Pressure Plate")]

public class PressurePlate : MonoBehaviour
{
    public TriggerInput trigger;

    private void Awake()
    {
        if (!trigger.pressurePlates.Contains(this))
        {
            trigger.pressurePlates.Add(this);
        }
    }



    public List<Weight> weights = new List<Weight>();
    bool isPressed;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.GetComponent<Weight>() != null)
        {
            if (!weights.Contains(collision.gameObject.GetComponent<Weight>()))
            {
                weights.Add(collision.gameObject.GetComponent<Weight>());
            }
        }
        updateWeight();
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<Weight>() != null)
        {
            if (weights.Contains(collision.gameObject.GetComponent<Weight>()))
            {
                weights.Remove(collision.gameObject.GetComponent<Weight>());
            }
        }
        updateWeight();
    }

    void updateWeight()
    {
        if (isPressed)
        {
            if(weights.Count == 0)
            {
                isPressed = false;
                trigger.Toggle(isPressed);
            }
        }
        else
        {
            if (weights.Count > 0)
            {
                isPressed = true;
                trigger.Toggle(isPressed);
    
            }
        }

    }






}
