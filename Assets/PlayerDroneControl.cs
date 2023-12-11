using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDroneControl : MonoBehaviour
{
    public void GivePlayerDrone()
    {
        GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().ToggleDroneDraft();
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
