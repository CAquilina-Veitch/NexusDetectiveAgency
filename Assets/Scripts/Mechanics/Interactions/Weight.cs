using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Nexus Detective Agency Components/ Properties/ Weight")]

public class Weight : MonoBehaviour
{
    public PressurePlate currentPlate;

    public void IsGrabbed()
    {
        if(currentPlate != null)
        {
            currentPlate.WeightGrabbed(this);
            currentPlate = null;
        }

    }

}
