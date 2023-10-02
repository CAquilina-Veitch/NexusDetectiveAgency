using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Player[] players = new Player[2];
    ref Player currentPlayer
    {
        get
        {
            return ref players[currentPlayerDimension];
        }
    }
    public int currentPlayerDimension;

    [SerializeField] Vector2 mouseInput;
    [SerializeField] float sensitivity = 1;
    float pitch, yaw;

    [SerializeField] Vector2 moveInput;
    [SerializeField] float speed = 8;
    [SerializeField] float acceleration = 10;

    [SerializeField] Rigidbody rb;
/*    ref Rigidbody rb
    {
        get { return ref rigidbodys[currentPlayerDimension]; }
    }*/

    [SerializeField] Vector3 dimensionalDiffPosition = new Vector3(-10,0,0);

    public void Toggle(bool to)
    {
        Cursor.lockState = to ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !to;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(dimensionalDiffPosition, 1);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(-dimensionalDiffPosition, 1);
    }

    private void Awake()
    {
        switchDimension(0);
        players[0].transform.position = dimensionalDiffPosition;
        players[1].transform.position = -dimensionalDiffPosition;
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

    public void ShowMouse(bool to){
        Toggle(!to);
    }

    public void switchDimension()
    {
        switchDimension(currentPlayerDimension == 1 ? 0 : 1);
    }
    void switchDimension(int to)
    {
        currentPlayerDimension = to;
        for(int i = 0; i < 2; i++)
        {
            players[i].SwitchTo(i == currentPlayerDimension);
            //rigidbodys[i].isKinematic = (i != currentPlayerDimension);
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            switchDimension();
        }
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y")) * sensitivity;
        pitch += mouseInput.y;
        yaw += mouseInput.x;
        pitch = Mathf.Clamp(pitch, -90, 90);
        foreach(Player p in players)
        {
            p.transform.rotation = Quaternion.Euler(0, yaw, 0);
            p.camTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
        }
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        Vector3 movementCalc = Vector3.Lerp(rb.velocity, currentPlayer.transform.forward * speed * moveInput.y + currentPlayer.transform.right * speed * moveInput.x, Time.deltaTime * acceleration);
        movementCalc.y = rb.velocity.y;
        rb.velocity = movementCalc;
        //Debug.Log(Time.deltaTime * acceleration);




    }
}
