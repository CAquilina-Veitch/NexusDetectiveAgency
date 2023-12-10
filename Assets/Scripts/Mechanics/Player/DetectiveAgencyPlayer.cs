using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DetectiveAgencyPlayer : MonoBehaviour
{
    [Space(20)]
    [Header("Dependencies")]
    [Space(10)]

    [SerializeField] Rigidbody rb;
    [SerializeField] Transform camTransform;
    LoreInventory loreInv;

    [Space(20)]
    [Header("Mechanisms")]
    [Space(10)]

    float pitch, yaw;

    [Space(10)]
    public Dimension currentPlayerDimension;
    [SerializeField] public Vector3 dimensionalDiffPosition = new Vector3(-10, 0, 0);
    [SerializeField] LayerMask groundMask;

    [Space(20)]
    [Header("Controls")]
    [Space(10)]

    [SerializeField] Vector2 mouseInput;
    [SerializeField, Range(0, 10)] float sensitivity = 0;

    [Space(20)]
    [Header("Movement")]
    [Space(10)]


    [SerializeField] Vector2 moveInput;
    [SerializeField, Min(0f)] float speed = 0;
    [SerializeField, Min(0.01f)] float acceleration = 10;

    public void Toggle(bool to)
    {
        Cursor.lockState = to ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !to;
    }

    private void OnEnable()
    {
        Toggle(true);
        yaw = transform.eulerAngles.y;

    }
    private void OnDisable()
    {
        Toggle(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowMouse(bool to)
    {
        Toggle(!to);
    }

    void Update()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y")) * sensitivity;
        pitch += mouseInput.y;
        yaw += mouseInput.x;
        pitch = Mathf.Clamp(pitch, -90, 90);
        transform.rotation = Quaternion.Euler(0, yaw, 0);
        camTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 movementCalc = Vector3.Lerp(rb.velocity, transform.forward * speed * moveInput.y + transform.right * speed * moveInput.x, Time.deltaTime * acceleration);
        movementCalc.y = rb.velocity.y;
        rb.velocity = movementCalc;
    }


    public void EnableControls()
    {
        sensitivity = 1;
        speed = 8;
    }
    public void DisableControls()
    {
        sensitivity = 0;
        speed = 0;
    }
    public void EnableControls(bool to)
    {
        if (to)
        {
            EnableControls();
        }
        else
        {
            DisableControls();
        }
    }


    private void Start()
    {
        loreInv = GameObject.FindGameObjectWithTag("LoreInventory").GetComponent<LoreInventory>();
        loreInv.Set(this);
        Toggle(true);
    }



}
