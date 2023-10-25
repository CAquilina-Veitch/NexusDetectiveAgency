using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    ref Player otherPlayer
    {
        get
        {
            return ref players[1-(int)currentPlayerDimension];
        }
    }
    public UIManager UIM
    {
        get
        {
            if (uiManager == null)
            {
                uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            }
            return uiManager;
        }
    }
    UIManager uiManager;

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
    [SerializeField] public Vector3 startingPos = new Vector3(31.3f, 1, 74.5f);
    [SerializeField] LayerMask groundMask;

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
    bool isGrounded
    {
        get
        {
            Debug.DrawRay(currentPlayer.transform.position, Vector3.down * jumpCheckLength, Color.cyan, 5.0f);
            if (Physics.Raycast(currentPlayer.transform.position,Vector3.down,jumpCheckLength, groundMask))
            {
                Debug.Log("EEE");
                return true;
            }
            else
            {
                Debug.Log("DDD");
                return false;
            }
        }
    }
    [SerializeField] float jumpCheckLength;


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


    public bool hasFuse;


    [Space(10)]
    public List<TriggerableObject> CollectedActions = new List<TriggerableObject>();



    public void Toggle(bool to)
    {
        Cursor.lockState = to ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !to;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(dimensionalDiffPosition+transform.position, 0.2f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(-dimensionalDiffPosition + transform.position, 0.2f);
    }

    private void Awake()
    {
        switchDimension(0);

        TransformPlayerParent(startingPos);
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
        //check valid Position

        RaycastHit hit;
        if (Physics.Raycast(otherPlayer.transform.position, Vector3.down, out hit, Mathf.Infinity,groundMask))
        {
            Debug.Log($"Did Hit Floor {hit.collider.gameObject.name}, distance is {hit.distance}");
            //there is floor somewhere there

            if (hit.distance > 0.1f)
            {
                //player head would not be directly inside an object

                currentPlayerDimension = to;
                for (int i = 0; i < 2; i++)
                {
                    players[i].SwitchTo(i == (int)currentPlayerDimension);
                    //rigidbodys[i].isKinematic = (i != currentPlayerDimension);
                }
                ChangeGrabParent();

            }

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
            currentPlayer.anim.SetTrigger("Vault");
            return true;
        }
        return false;
        
    }
    void Jump()
    {
        if (isGrounded)
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
                    currentPlayer.anim.SetTrigger("Grab");
                    currentHeldItem = hit.collider.GetComponent<Holdable>();
                    currentHeldItem.ToggleGhost(true);
                    ChangeGrabParent();
                    if (hit.collider.GetComponent<Weight>() != null)
                    {
                        hit.collider.GetComponent<Weight>().IsGrabbed();
                    }
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
                hit.collider.GetComponent<Button>().Activate();
            }
        }
    }



    //Abilities


    void Repair()
    {
        RaycastHit hit;
        if (Physics.Raycast(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward, out hit, armReach))
        {
            Debug.DrawRay(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward * hit.distance, Color.cyan, 5);
            Debug.Log($"Did Hit Fuse {hit.collider.gameObject.name}");
            if (hit.collider.GetComponent<Repairable>() != null)
            {
                hit.collider.GetComponent<Repairable>().TryRepair(this);
            }
        }
    }







   /*void DirectDrone()
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
    }*/
   



    //showWheel;


    void ShowWheel()
    {
        UIM.ToggleUI("Remote Interaction", true);
        ShowMouse(true);
        currentPlayer.anim.SetTrigger("HoverHand");

    }

    void HideWheel()
    {
        Debug.LogError(GetMouseOverNumber());
        UIM.ToggleUI("Remote Interaction", false);
        ShowMouse(false);
        currentPlayer.anim.SetTrigger("HoverDown");


    }

    float GetMouseOverNumber()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 mousePosition = Input.mousePosition;
        Vector3 direction = screenCenter - mousePosition;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        angle -= 90f;
        angle = 360 - angle;

        if (angle < 0)
        {
            angle += 360f;
        }

        // Add 90 degrees to the angle to make 0 point down and increase clockwise


        if (angle >= 360f)
        {
            angle -= 360f;
        }

        int num = CollectedActions.Count == 0 ? 1 : CollectedActions.Count;
        int fitCount = Mathf.FloorToInt(angle / num);
        return fitCount;
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
            if(Physics.Raycast(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward, out obstacleHit, heldItemArmLength + objectSize,groundMask))
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
            Repair();
        }


        if (currentPlayerDimension == Dimension.Cyberpunk)
        {

            if (Input.GetKeyDown(abilityOneKey))
            {
                switchDimension(Dimension.Steampunk);
            }
            if (Input.GetKeyDown(abilityTwoKey))
            {
                ShowWheel();
            }
            if (Input.GetKeyUp(abilityTwoKey))
            {
                HideWheel();
            }
        }
        else if (currentPlayerDimension == Dimension.Steampunk)
        {
            if (Input.GetKeyDown(abilityOneKey))
            {
                switchDimension(Dimension.Cyberpunk);
            }

            /*if (Input.GetKeyDown(abilityTwoKey))
            {
                DirectDrone();
            }*/
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
