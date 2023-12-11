using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelSpin : MonoBehaviour
{
    public void Spin()
    {
        StartCoroutine(spinOnDelay());
    }
    IEnumerator spinOnDelay()
    {
        yield return new WaitForSeconds(11.25f);
        transform.localRotation = Quaternion.EulerAngles(0,-90,0);
    }
}
