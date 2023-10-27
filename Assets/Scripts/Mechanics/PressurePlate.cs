using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Nexus Detective Agency Components/ Triggers/ Pressure Plate")]

public class PressurePlate : MonoBehaviour
{
    public TriggerableObject trigger;

    private void Awake()
    {
        if (!trigger.pressurePlates.Contains(this))
        {
            trigger.pressurePlates.Add(this);
        }
    }



    public List<Weight> weights = new List<Weight>();
    bool isPressed;

    private void OnTriggerEnter(Collider  collision)
    {
        if(collision.gameObject.GetComponent<Weight>() != null)
        {
            if (!weights.Contains(collision.gameObject.GetComponent<Weight>()))
            {
                weights.Add(collision.gameObject.GetComponent<Weight>());
                collision.gameObject.GetComponent<Weight>().currentPlate = this;
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
                collision.gameObject.GetComponent<Weight>().currentPlate = null;
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
                trigger.Triggered();
            }
        }
        else
        {
            if (weights.Count > 0)
            {
                isPressed = true;
                trigger.Triggered();

            }
        }

    }


    public void WeightGrabbed(Weight weight)
    {
        if (weights.Contains(weight))
        {
            weights.Remove(weight);
        }
        updateWeight();
    }



}
