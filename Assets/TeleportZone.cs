using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TeleportZone : MonoBehaviour
{
    [SerializeField] Transform tpPos;
    [SerializeField] Transform[] crusherHalfs;
    Vector3[] crusherHalfStarts = new Vector3[2];
    [SerializeField] Vector3 direction;
    [SerializeField] float speed;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            collision.gameObject.GetComponent<PlayerController>().Teleport(tpPos.position);
        }
        else
        {
            if (collision.gameObject.GetComponent<Holdable>())
            {
                collision.gameObject.GetComponent<Holdable>().Respawn();
            }
        }
    }

    private void OnEnable()
    {
        if (crusherHalfs.Length>0)
        {
            crusherHalfStarts[0] = crusherHalfs[0].position;
            crusherHalfStarts[1] = crusherHalfs[1].position;
        }
            
    }
    private void FixedUpdate()
    {
        if (crusherHalfs.Length>0)
        {
            crusherHalfs[0].position = crusherHalfStarts[0] + direction * Mathf.Sin(Time.time * speed);
            crusherHalfs[1].position = crusherHalfStarts[1] - direction * Mathf.Sin(Time.time * speed);
        }

    }

}
