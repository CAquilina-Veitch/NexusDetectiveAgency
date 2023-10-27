using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyInEditor : MonoBehaviour
{

    private void Awake()
    {
        if(!Application.isEditor)
        {
            Destroy(gameObject);
        }

    }
}
