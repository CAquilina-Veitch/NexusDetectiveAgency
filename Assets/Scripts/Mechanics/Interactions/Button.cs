using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[AddComponentMenu("Nexus Detective Agency Components/ Triggers/ Pressable Button")]


public class Button : MonoBehaviour
{
    public TriggerableObject trigger;
    [SerializeField] Animator animator;



    private void Awake()
    {
        if (!trigger.buttons.Contains(this))
        {
            trigger.buttons.Add(this);
        }
    }


    public void Activate()
    {
        trigger.Triggered();
        animator.SetTrigger("Pressed");
        
    }




}
