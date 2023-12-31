using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TurnOffPlayer: MonoBehaviour
{
    PlayerController pC;
    public GameObject[] obsOffTake;
    public GameObject[] obsOffActiv;
    [SerializeField] UnityEvent turnOnCanvas;
    public void StartAnim()
    {
        pC = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        pC.activateCameras(false);
    }

    IEnumerator onOnDelayTake(float d)
    {
        yield return new WaitForSeconds(d);
        pC.activateCameras(true);
        turnOnCanvas.Invoke();
        foreach(var c in obsOffTake)
        {
            c.SetActive(false);
        }
    }

    IEnumerator onOnDelayActiv(float d)
    {
        yield return new WaitForSeconds(d);
        pC.activateCameras(true);
        turnOnCanvas.Invoke();
        foreach (var c in obsOffActiv)
        {
            c.SetActive(false);
        }
    }

    public void PlayerBackOnTake(float d)
    {
        StartCoroutine(onOnDelayTake(d));
    }

    public void PlayerBackOnActiv(float d)
    {
        StartCoroutine(onOnDelayActiv(d));
    }
}
