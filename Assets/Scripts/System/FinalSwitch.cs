using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class FinalSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    VisualEffect swirl;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneSegmentManager>().FinalDetectiveAgency();
            StartVFX();
        }
    }
    public void StartVFX()
    {
        swirl = GameObject.FindGameObjectWithTag("Swirl").GetComponent<VisualEffect>();
        swirl.Play();
    }
}
