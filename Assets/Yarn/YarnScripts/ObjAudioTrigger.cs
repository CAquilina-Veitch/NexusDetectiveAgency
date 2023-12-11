using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjAudioTrigger : MonoBehaviour
{
    public int maxLineTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<MaxAudioTrigger>().maxLineNumber = maxLineTrigger;
            other.GetComponent<MaxAudioTrigger>().AudioTriggerEnter();
            Destroy(this.gameObject);
        }
    }
}
