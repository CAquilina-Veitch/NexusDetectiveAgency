using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffPlayer: MonoBehaviour
{
    PlayerController pC;
    public GameObject[] obsOff;
    public void StartAnim()
    {
        pC = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        pC.activateCameras(false);
    }

    IEnumerator onOnDelay(float d)
    {
        yield return new WaitForSeconds(d);
        pC.activateCameras(true);
        foreach(var c in obsOff)
        {
            c.SetActive(false);
        }
    }

    public void PlayerBackOn(float d)
    {
        StartCoroutine(onOnDelay(d));
    }
}
