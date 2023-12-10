using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upright : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
