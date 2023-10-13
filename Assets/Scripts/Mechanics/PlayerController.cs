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
    [SerializeField] KeyCode grabHoldableKey = KeyCode.Mouse0;
    [SerializeField] KeyCode buttonPressKey = KeyCode.Mouse1;



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

    [Space(10)]
    public Holdable currentHeldItem;
    [SerializeField] float armReach = 4;
    [SerializeField] float heldItemArmLength = 3;


    [Space(20)]
    [Header("Abilities")]
    [Space(10)]
    [SerializeField] GameObject dronePlatformPrefab;
    [SerializeField] GameObject droneDraft;
    bool dronePlatformActive;
    bool dronePlatformDrafting;
    [HideInInspector] public float dronePlatformDistance;
    GameObject currentDronePlatformPrefab;

    [Space(10)]
    public List<Button> CollectedButtons = new List<Button>();



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



    void switchDimension(Dimension to)
    {
        currentPlayerDimension = to;
        for(int i = 0; i < 2; i++)
        {
            players[i].SwitchTo(i == (int)currentPlayerDimension);
            //rigidbodys[i].isKinematic = (i != currentPlayerDimension);
        }
        ChangeGrabParent();

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

    void GrabHoldableItem()
    {
        if (currentHeldItem == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward, out hit, armReach))
            {
                Debug.DrawRay(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward* hit.distance, Color.yellow,5);
                Debug.Log($"Did Hit Grabbable {hit.collider.gameObject.name}");
                if(hit.collider.GetComponent<Holdable>() != null )
                {
                    currentHeldItem = hit.collider.GetComponent<Holdable>();
                    currentHeldItem.ToggleGhost(true);
                    ChangeGrabParent();
                }
            }
        }
        else
        {
            currentHeldItem.transform.parent = null;
            currentHeldItem.ToggleGhost(false);
            currentHeldItem = null;
        }
    }

    void ChangeGrabParent()
    {
        if (currentHeldItem != null)
        {
            currentHeldItem.transform.parent = currentPlayer.cam.transform;
            currentHeldItem.transform.position = currentPlayer.cam.transform.position + currentPlayer.cam.transform.forward * heldItemArmLength;
        }
        
    }

    void PressButton()
    {
        RaycastHit hit;
        if (Physics.Raycast(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward, out hit, armReach))
        {
            Debug.DrawRay(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward * hit.distance, Color.cyan,5);
            Debug.Log($"Did Hit Button {hit.collider.gameObject.name}");
            if (hit.collider.GetComponent<Button>() != null)
            {
                hit.collider.GetComponent<Button>().Trigger();
            }
        }
    }



    //Abilities










    void DirectDrone()
    {
        dronePlatformDrafting = !dronePlatformDrafting;

        if (dronePlatformDrafting)
        {
            ShowDraft(true);
        }
        else
        {
            NewDronePositionSet();
        }
        
    }




    void NewDronePositionSet()
    {
        ShowDraft(false);
        Destroy(currentDronePlatformPrefab);

    }

    void ShowDraft(bool showDraft)
    {
        dronePlatformDrafting = showDraft;

        droneDraft.SetActive(showDraft);

        dronePlatformDistance = 5;
        if (showDraft)
        {

        }
        else
        {
            dronePlatformDistance = 5;
        }
    }




    //showWheel;
















    ///////////////////////////////////////////////////
    

    void TransformPlayerParent(Vector3 to)
    {
        transform.position = to;
        for (int i = 0; i < 2; i++)
        {
            players[i].transform.position = to.OwnerPosToPlayer(this)[i];
        }
    }


    void FixedUpdate()
    {
        if (currentHeldItem != null)
        {
            
            Vector3 grabPosition = currentPlayer.cam.transform.position + currentPlayer.cam.transform.forward * heldItemArmLength;

            // Calculate the object's size
            Bounds objectBounds = currentHeldItem.cldrBounds;
            float objectSize = Mathf.Max(objectBounds.size.x, objectBounds.size.y, objectBounds.size.z);
            
            RaycastHit obstacleHit;
            // Check for obstacles between the camera and the grab position
            if(Physics.Raycast(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward, out obstacleHit, heldItemArmLength + objectSize))
            {
                // Calculate a new grab position considering the object's size
                float adjustedArmLength = Mathf.Max(Vector3.Distance(currentPlayer.cam.transform.position, obstacleHit.point) - objectSize, 0f);
                //adjustedArmLength -= objectSize;
                grabPosition = currentPlayer.cam.transform.position + currentPlayer.cam.transform.forward * adjustedArmLength;
            }

            currentHeldItem.transform.position = grabPosition;
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
        if(Input.GetKeyDown(grabHoldableKey))
        {
            GrabHoldableItem();
        }
        
        
        if(Input.GetKeyDown(buttonPressKey))
        {
            PressButton();
        }


        if (currentPlayerDimension == Dimension.Cyberpunk)
        {
            if (Input.GetKeyDown(abilityOneKey))
            {
                switchDimension(Dimension.Steampunk);
            }            
            
            if (Input.GetKeyDown(abilityTwoKey))
            {
                DirectDrone();
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
