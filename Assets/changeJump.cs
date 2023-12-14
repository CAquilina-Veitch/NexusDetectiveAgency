using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeJump : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("PlayerController"))
        {
            GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().jumpHeight = 6;
        }
    }

}
