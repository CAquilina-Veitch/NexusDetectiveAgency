using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartVolume : MonoBehaviour
{
    Slider sl;

    private void Start()
    {
        sl = GetComponent<Slider>();
        sl.onValueChanged.Invoke(0.5f);
    }
}
