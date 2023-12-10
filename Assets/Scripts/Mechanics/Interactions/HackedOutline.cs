using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HackedOutline : MonoBehaviour
{
    // Start is called before the first frame update
    public Material HackMat;
    MeshRenderer mR;
    private bool HasBeen;
    private void Start()
    {
        mR = GetComponent<MeshRenderer>();
    }

    public void Hacked()
    {
        if(HasBeen == false)
        {
            var temp = mR.materials.ToList();
            temp.Add(HackMat);
            mR.SetMaterials(temp);
            HasBeen = true;
        }
    }
}
