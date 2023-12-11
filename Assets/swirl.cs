using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class swirl : MonoBehaviour
{
    public Camera cam;
    public VisualEffect vfct;


    public void Swirl()
    {
        StartCoroutine(swirlAnim());
    }

    IEnumerator swirlAnim()
    {
        cam.enabled = true;
        vfct.Play();
        yield return new WaitForSeconds(3);
        cam.enabled = false;
    }


}
