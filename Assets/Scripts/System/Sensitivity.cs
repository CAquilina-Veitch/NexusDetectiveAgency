using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensitivity : MonoBehaviour
{


    DetectiveAgencyPlayer detective;
    PlayerController player;
    bool isDetective;

    float boundSens;

    public void ChangeSensitivity(float sensitivity)
    {
        boundSens = sensitivity;
        if (isDetective)
        {

        }
    }

    public float Set(DetectiveAgencyPlayer p)
    {
        isDetective = true;
        detective = p;
        return boundSens;
    }
    public float Set(PlayerController p)
    {
        isDetective = false;
        player = p;
        return boundSens;
    }
}
