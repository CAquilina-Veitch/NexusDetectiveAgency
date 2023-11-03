using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Nexus Detective Agency Components/ Mechanical Door")]

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(TriggerableObject))]





public class MechanicalDoor : MonoBehaviour
{
    [TextArea(1,20)]
    [Tooltip("Doesn't do anything. Just comments shown in inspector")]
     public string Note = "This is a 'mechanical' door, meaning its triggered remotely and cant be opened by hand.\r\n\r\nTimeOpen controls how long the door will stay open before closing, set the number to 0 if you want it to stay open indefinitely.\r\n\r\nConnect this components ToggleOpen to the triggerable object event, either to toggle or specify open/close with bool parameter\r\n\r\n\r\nThis component will need a collider to operate; can be any type. ";

    bool isOpen;
    Collider cldr;

    [Space(10)]
    [SerializeField] float openTime;
    bool isClosing;
    [Space(10)]



    [Header("Open & Close Actions")]
    [Space(10)]
    public UnityEvent openAction;
    [Space(10)]

    public UnityEvent closeAction;

    [SerializeField] Animator animator;

    private void Awake()
    {
        cldr = GetComponent<Collider>();
    }





    public void ToggleOpen()
    {
        ToggleOpen(!isOpen);
    }
    public void ToggleOpen(bool to)
    {
        isOpen = to;
        cldr.enabled = !isOpen;//cud change

        if(isOpen)
        {
            openAction.Invoke();
            animator.SetTrigger("Open");
            if(isClosing )
            {
                StopCoroutine(Reclose());
            }
            StartCoroutine(Reclose());
        }
        else
        {
            animator.SetTrigger("Close");
            closeAction.Invoke();
        }




    }
    IEnumerator Reclose()
    {
        if(openTime>0)
        {
            isClosing = true;
            yield return new WaitForSeconds(openTime);
            ToggleOpen(false);
        }
        
    }





}
