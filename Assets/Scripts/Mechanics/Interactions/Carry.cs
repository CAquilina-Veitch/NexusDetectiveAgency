using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Carry : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if(other != null)
        {
            if(other.GetComponent<Rigidbody>() != null)
            {
                other.transform.SetParent(transform, true);
            }
            else
            {
                if (other.GetComponentInParent<Rigidbody>() != null)
                {
                    other.GetComponentInParent<Rigidbody>().transform.SetParent(transform, true);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            if (other.GetComponent<Rigidbody>() != null)
            {
                other.transform.SetParent(null, true);
            }
            else
            {
                if (other.GetComponentInParent<Rigidbody>() != null)
                {
                    other.GetComponentInParent<Rigidbody>().transform.SetParent(null, true);
                }
            }
        }
        if (other.tag == "Player"/*|| other.tag=="PlayerController"*/)
        {
            SceneManager.MoveGameObjectToScene(other.GetComponentInParent<PlayerController>().gameObject, SceneManager.GetSceneByBuildIndex(2));
        }
    }

}
