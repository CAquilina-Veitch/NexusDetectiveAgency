using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[AddComponentMenu("Nexus Detective Agency Components/ Triggers/ Pressable Button")]
[RequireComponent(typeof(SoundEmitter))]

public class Button : MonoBehaviour
{
    public TriggerableObject trigger;
    [SerializeField] Animator animator;
    [SerializeField] SoundEmitter soundEmitter;


    private void Awake()
    {
        if (!trigger.buttons.Contains(this))
        {
            trigger.buttons.Add(this);
        }
    }


    public void Activate()
    {
        soundEmitter.StartSound();
        trigger.Triggered();
        animator.SetTrigger("Pressed");
        
    }




}
