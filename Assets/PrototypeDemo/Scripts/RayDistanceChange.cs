using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class UICollision : MonoBehaviour
{
    [SerializeField] UnityEvent Interact;
    [SerializeField] UnityEvent Leave;
    public PlayerInteract playerInteract;
    public float[] distances =
    {
        8,16
    };
    // Start is called before the first frame update
    private void OnEnable()
    {
        playerInteract = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteract>();
    }
    private void OnTriggerEnter(Collider Player)
    {
        playerInteract.distance = distances[1];
    }
        private void OnTriggerExit(Collider Player)
    {
        playerInteract.distance = distances[0];
    }
}
