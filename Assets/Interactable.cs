using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public float viewTimer = 0.1f;
    float currentTimer;
    bool isOverlayed;

    MeshRenderer mR;
    public Material outlineMat;

    private void Start()
    {
        mR = GetComponent<MeshRenderer>();
    }
    
    private void FixedUpdate()
    {
        if(isOverlayed)
        {
            if (currentTimer > 0)
            {
                currentTimer -= Time.deltaTime;
            }
            else
            {
                Debug.Log("###");
                var temp = mR.materials.ToList();
                temp.RemoveAt(temp.Count - 1);
                mR.SetMaterials(temp);
                isOverlayed = false;
            }
        }
    }


    public void Seen()
    {
        currentTimer = viewTimer;
        if (!isOverlayed)
        {
            isOverlayed = true;


            var temp = mR.materials.ToList();
            temp.Add(outlineMat);
            mR.SetMaterials(temp);
        }
    }
}
