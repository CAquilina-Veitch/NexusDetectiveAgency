using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Dimension { Cyberpunk, Steampunk, Noir}

public class PlayerController : MonoBehaviour
{
    
    [Space(20)]
    [Header("Dependencies")]
    [Space(10)]

    [SerializeField] Rigidbody rb;
    
    
    [SerializeField] Player[] players = new Player[2];
    ref Player currentPlayer
    {
        get
        {
            return ref players[(int)currentPlayerDimension];
        }
    }


    [Space(20)]
    [Header("Keybinds")]
    [Space(10)]

    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode abilityOneKey = KeyCode.E;
    [SerializeField] KeyCode abilityTwoKey = KeyCode.Q;
    [SerializeField] KeyCode abilityThreeKey = KeyCode.F;



    [Space(20)]
    [Header("Mechanisms")]
    [Space(10)]

    float pitch, yaw;
   
    [Space(10)]
    public Dimension currentPlayerDimension;
    [SerializeField] public Vector3 dimensionalDiffPosition = new Vector3(-10, 0, 0);
    
    [Space(20)]
    [Header("Controls")]
    [Space(10)]

    [SerializeField] Vector2 mouseInput;
    [SerializeField,Range(0,10)] float sensitivity = 1;

    [Space(20)]
    [Header("Movement")]
    [Space(10)]


    [SerializeField] Vector2 moveInput;
    [SerializeField,Min(0.01f)] float speed = 8;
    [SerializeField, Min(0.01f)] float acceleration = 10;
    float isGrounded;

    [Space(10)]
    [HideInInspector] public Ledge currentLedge;
    [SerializeField, Min(0f)] float maxVaultAngle = 45;
    [Space(20)]
    [Header("Abilities")]
    [Space(10)]
    [SerializeField] GameObject dronePlatformPrefab;


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

        TransformPlayerParent(transform.position);
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

/*    public void switchDimension()
    {
        switchDimension(currentPlayerDimension == (Dimension)1 ? 0 : 1);
    }*/
    void switchDimension(Dimension to)
    {
        currentPlayerDimension = to;
        for(int i = 0; i < 2; i++)
        {
            players[i].SwitchTo(i == (int)currentPlayerDimension);
            //rigidbodys[i].isKinematic = (i != currentPlayerDimension);
        }

    }

    //Movement

    bool Vault()
    {
        Vector3 playerLookDirection = currentPlayer.camTransform.forward;
        Vector3 desiredDirection = currentLedge.direction;
        float angle = Vector3.Angle(playerLookDirection, desiredDirection);

        if (angle<maxVaultAngle)   //facing right way
        {
            StartCoroutine(LerpVault(currentLedge.transform.position + Vector3.up * 2));
            return true;
        }
        return false;
        
    }
    void Jump()
    {
        if (isGrounded==0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 7, rb.velocity.z);
        }
    }

    IEnumerator LerpVault(Vector3 targetPos)
    {
        Vector3 targetPosParent = targetPos.PlayerPosToOwner(this);
        float duration = 1;
        float time = 0;
        Vector3 startPosition = transform.position;
        Debug.Log($"from {startPosition} to {targetPosParent}");
        while (time < duration)
        {
            TransformPlayerParent(Vector3.Lerp(startPosition, targetPosParent, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        TransformPlayerParent(targetPosParent);
    }

    //Abilities







    bool dronePlatformActive;
    bool dronePlatformDrafting;
    [Range(0, 20)] float dronePlatformDistance;


    void DirectDrone()
    {
        dronePlatformDrafting = !dronePlatformDrafting;
        if (dronePlatformDrafting)
        {

        }
        else
        {

        }
        
    }




    void newDronePositionSet()
    {

    }

    ///////////////////////////////////////////////////
    

    void TransformPlayerParent(Vector3 to)
    {
        transform.position = to;
        for (int i = 0; i < 2; i++)
        {
            players[i].transform.position = to.OwnerPosToPlayer(this)[i];
        }
    }







    void Update()
    {

        if (Input.GetKeyDown(jumpKey))
        {
            if (currentLedge == null)
            {
                Jump();
            }
            else
            {
                if (!Vault())
                {
                    Jump();
                }
            }
            
        }
        if (currentPlayerDimension == Dimension.Cyberpunk)
        {
            if (Input.GetKeyDown(abilityOneKey))
            {
                switchDimension(Dimension.Steampunk);
            }            
            
            if (Input.GetKeyDown(abilityTwoKey))
            {

            }

        }
        else if (currentPlayerDimension == Dimension.Steampunk)
        {
            if (Input.GetKeyDown(abilityOneKey))
            {
                switchDimension(Dimension.Cyberpunk);
            }
            if (Input.GetKeyDown(abilityTwoKey))
            {

            }
        }
        else
        {

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
    }

}
