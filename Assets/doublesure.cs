using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class doublesure : MonoBehaviour
{
    private void Start()
    {
        GameObject.FindGameObjectWithTag("Fadeblack").GetComponent<Fadeblack>().loadingScreen.alpha = 0;
        GameObject.FindGameObjectWithTag("Fadeblack").GetComponent<Fadeblack>().camOff();
    }
}
