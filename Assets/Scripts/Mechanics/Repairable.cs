using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Nexus Detective Agency Components/ Triggers/ Repairable")]

public class Repairable : MonoBehaviour
{
    public bool hasFuse;

    [HideInInspector] public TriggerInput trigger;
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
                this.hasFuse = false;
                pC.hasFuse = true;
                if(trigger != null)
                {
                    trigger.updateRepairs();
                }

                fuseObj.SetActive(false);
                Debug.Log(2);
            }

        }
        else
        {
            if (pC.hasFuse)
            {
                pC.hasFuse = false;
                hasFuse = true;
                fuseObj.SetActive(true);
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
