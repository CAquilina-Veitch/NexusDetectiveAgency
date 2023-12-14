using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchFromPlayer : MonoBehaviour
{
    PlayerController pC;
    [SerializeField] Camera cam;
    [SerializeField] DialogueManager dM;
    public GameObject hands;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            pC = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
            TogglePlayerCam(false);
            this.enabled = false;
        }
    }

    public void TogglePlayerCam(bool to)
    {
        pC.activateCameras(to);
        cam.enabled = !to;

        hands.SetActive(!to);


        Cursor.lockState = to ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !to;

        GetComponent<BoxCollider>().enabled = false;
        if (!to)
        {
            dM.StartMelConvo();
        }
    }

    public void Deactivate()
    {
        cam.enabled = false;
    }

    public void MaxType()
    {
        anim.SetTrigger("Typing");
    }
    public Animator anim;

    public void MelFlashToReal()
    {

    }


}
