using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelSpin : MonoBehaviour
{
    bool spun;
    public void Spin()
    {

        if(!spun )
        {
            spun = true;
            StartCoroutine(spinOnDelay());
        }

    }
    public GameObject[] toTurnOff;
    IEnumerator spinOnDelay()
    {
        yield return new WaitForSeconds(11.25f);
        transform.localRotation = Quaternion.Euler(0,-90,0);
        foreach(GameObject go in toTurnOff)
        {
            go.SetActive(false);
        }
    }
}
