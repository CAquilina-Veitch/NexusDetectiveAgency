using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Nexus Detective Agency Components/ PressurePlate")]

public class PressurePlate : MonoBehaviour
{

    public List<Weight> weights = new List<Weight>();
    bool isPressed;

    private void OnCollisionEnter(Collision collision)
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
    private void OnCollisionExit(Collision collision)
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
            }
        }
        else
        {
            if (weights.Count > 0)
            {
                isPressed = true;
            }
        }

    }






}
