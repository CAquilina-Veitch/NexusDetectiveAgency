using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDroneControl : MonoBehaviour
{
    public void GivePlayerDrone()
    {
        Debug.Log("egg");
        GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().ToggleDroneDraft();
    }

    public void GivePlayerDrone(bool to)
    {
        Debug.Log(to);
        GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().ToggleDroneDraft(to);
    }

    public bool happenOnTrigger = false;
    public bool onTriggerGivePlayerDroneIfTrueOrTakeIfFalse = true;

    private void OnTriggerEnter(Collider other)
    {
        if (happenOnTrigger)
        {
            if (other.tag == "Player")
            {
                GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().ToggleDroneDraft(onTriggerGivePlayerDroneIfTrueOrTakeIfFalse);
            }
        }
    }

}
