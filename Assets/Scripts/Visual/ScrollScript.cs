using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCanvas : MonoBehaviour
{
    public float startPosition;
    public float currentScrollAmount = 0f;
    public float scrollSpeed = -1f;
    public float maxLength = 800f; // Change this value as needed

    void Start()
    {
        startPosition = transform.localPosition.y;
    }

    void Update()
    {
        float scrollDelta = Input.mouseScrollDelta.y;

        if (scrollDelta != 0)
        {
            currentScrollAmount = Mathf.Clamp(currentScrollAmount + (scrollDelta * scrollSpeed), 0, maxLength);

            transform.localPosition = new Vector3(transform.localPosition.x, currentScrollAmount+startPosition, transform.localPosition.z);
        }
    }
}