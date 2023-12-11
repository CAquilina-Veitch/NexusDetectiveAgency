using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinalSwitch : MonoBehaviour
{
    // Start is called before the first frame update


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneSegmentManager>().FinalDetectiveAgency();
        }
    }
}
