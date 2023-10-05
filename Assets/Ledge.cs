using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Windows;

[AddComponentMenu("Nexus Detective Agency Components/ Ledge")]
public class Ledge : MonoBehaviour
{
    [SerializeField] BoxCollider collisionBox;
    [SerializeField] public Vector2 direction;


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position + multiplyByLocalScale(collisionBox.center) , multiplyByLocalScale(collisionBox.size));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + multiplyByLocalScale(collisionBox.center) + direction.toVector3()+Vector3.up, 1);
    }



    Vector3 multiplyByLocalScale(Vector3 input)
    {
        return new Vector3(input.x * transform.lossyScale.x, input.y * transform.lossyScale.y, input.z * transform.lossyScale.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.tag == "Player")
        {
            other.GetComponentInParent<PlayerController>().currentLedge = this;
        }
    }    
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.tag == "Player")
        {
            if (other.GetComponentInParent<PlayerController>().currentLedge == this)
            {
                other.GetComponentInParent<PlayerController>().currentLedge = null;
            }
            
        }
    }
}
