using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportZone : MonoBehaviour
{
    [SerializeField] Transform tpPos;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            collision.gameObject.GetComponent<PlayerController>().Teleport(tpPos.position);
        }
    }
}
