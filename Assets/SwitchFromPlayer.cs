using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchFromPlayer : MonoBehaviour
{
    PlayerController pC;
    [SerializeField] Camera cam;
    [SerializeField] DialogueManager dM;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            pC = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
            TogglePlayerCam(false);
        }
    }

    public void TogglePlayerCam(bool to)
    {
        pC.gameObject.SetActive(to);
        cam.enabled = !to;
        GetComponent<BoxCollider>().enabled = false;
        if (!to)
        {
            StartDialogue();
        }
    }
    void StartDialogue()
    {
        dM.StartMelConvo();
    }

    public void Deactivate()
    {
        cam.enabled = false;
    }



}
