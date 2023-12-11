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
        if (other.tag == "Player")
        {
            pC = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        }
    }

    void TogglePlayerCam(bool to)
    {
        pC.gameObject.SetActive(to);
        cam.enabled = !to;
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
        GetComponent<BoxCollider>().enabled = false;
        cam.enabled = false;
    }



}
