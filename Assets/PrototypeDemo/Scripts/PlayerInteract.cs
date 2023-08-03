using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform cam;
    [SerializeField] 
    public float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    void Start()
    {
        //cam = GetComponent<PlayerController>().cam;
        playerUI = GetComponent<PlayerUI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(cam.position, cam.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if (Physics.Raycast (ray, out hitInfo, distance, mask))
        {
            if(hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage);
                if (Input.GetKeyDown("e"))
                {
                    interactable.BaseInteract();
                }
            }
        }
    }
}
