using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDroneControl : MonoBehaviour
{
    public void GivePlayerDrone(bool to)
    {
        GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().HasDrone(to);
    }

    public bool happenOnTrigger = false;
    public bool onTriggerGivePlayerDroneIfTrueOrTakeIfFalse = true;

    private void OnTriggerEnter(Collider other)
    {
        if (happenOnTrigger)
        {
            if (other.tag == "Player")
            {
                GivePlayerDrone(onTriggerGivePlayerDroneIfTrueOrTakeIfFalse);
            }
        }
    }

}
