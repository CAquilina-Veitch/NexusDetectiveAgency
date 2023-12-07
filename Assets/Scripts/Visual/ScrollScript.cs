using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCanvas : MonoBehaviour
{
    public float startPosition;
    public float currentScrollAmount = 0f;
    public float scrollAmount;
    public float scrollSpeed = 1f;
    public float maxLength = 800f; // Change this value as needed

    void Start()
    {
        startPosition = transform.position.y;
    }

    void Update()
    {
        float scrollDelta = Input.mouseScrollDelta.y;

        if (scrollDelta != 0)
        {
            scrollAmount = Mathf.Clamp(currentScrollAmount + (scrollDelta * scrollSpeed), 0, maxLength);

            transform.position = new Vector3(transform.position.x, currentScrollAmount, transform.position.z);
        }
    }
}