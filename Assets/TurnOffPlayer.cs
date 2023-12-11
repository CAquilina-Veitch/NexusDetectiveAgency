using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffPlayer: MonoBehaviour
{
    PlayerController pC;

    public void StartAnim()
    {
        pC = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        pC.gameObject.SetActive(false);
    }

}
