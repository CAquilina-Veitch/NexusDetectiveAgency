using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCarrier : MonoBehaviour
{
    public int endingChosen = -1;
    public void ChoseEnding(int i)
    {
        endingChosen = i;
    }
}
