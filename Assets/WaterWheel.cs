using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWheel : MonoBehaviour
{
    [SerializeField] float spinSpeed;
    float currentSpinSpeed;
    [SerializeField] bool currentlySpinning;
    private void FixedUpdate()
    {
        if (currentSpinSpeed > 0)
        {
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * currentSpinSpeed));
        }
        currentSpinSpeed = Mathf.Lerp(currentSpinSpeed, currentlySpinning ? spinSpeed : 0, Time.deltaTime);
    }

    public void toggle()
    {
        currentlySpinning = !currentlySpinning;
    }
}
