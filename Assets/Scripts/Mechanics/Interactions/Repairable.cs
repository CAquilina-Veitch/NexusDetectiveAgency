using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[AddComponentMenu("Nexus Detective Agency Components/ Triggers/ Repairable")]

public class Repairable : MonoBehaviour
{
    [SerializeField] SoundEmitter soundEmitterPlug;
    [SerializeField] SoundEmitter soundEmitterUnplug;
    public bool hasFuse;
    [SerializeField] UnityEvent powered;
    [SerializeField] UnityEvent unpowered;

    [HideInInspector] public TriggerableObject trigger;
    [SerializeField] GameObject fuseObj;

    private void Awake()
    {
        fuseObj.SetActive(hasFuse);
    }

    public void TryRepair(PlayerController pC)
    {
        Debug.Log($"player fuse {pC.hasFuse}, this has fuse {hasFuse}");
        if (hasFuse)
        {
            if (pC.hasFuse)
            {
                //is already repaired and player has one, show ui for full inv / no space
                Debug.Log(1);
            }
            else
            {
                soundEmitterUnplug.StartSound();
                this.hasFuse = false;
                pC.hasFuse = true;
                if(trigger != null)
                {
                    trigger.UpdateRepairs();
                }
                powered.Invoke();
                fuseObj.SetActive(false);
                Debug.Log(2);
            }

        }
        else
        {
            if (pC.hasFuse)
            {
                soundEmitterPlug.StartSound();
                pC.hasFuse = false;
                hasFuse = true;
                fuseObj.SetActive(true);
                if (trigger != null)
                {
                    trigger.UpdateRepairs();
                }
                unpowered.Invoke();
                //Repair();
                Debug.Log(3);
            }
            else
            {
                //show ui for no fuse
                Debug.Log(4);
            }
        }

    }


}
