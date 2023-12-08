using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.VFX;

public enum Dimension { Cyberpunk, Steampunk, Noir}

public enum GroundType { normal, concrete, metal, wood, dirt}
public class PlayerController : MonoBehaviour
{
    GameObject lastInteracted;
    [Space(20)]
    [Header("Dependencies")]
    [Space(10)]

    [SerializeField] Rigidbody rb;

    [SerializeField] LoreInventory loreInv;
    [SerializeField] CanvasGroup inventoryCanvasGroup;
    [SerializeField] Player[] players = new Player[2];
    public ref Player currentPlayer
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
    [SerializeField] KeyCode dimensionSwapKey = KeyCode.E;
    [SerializeField] KeyCode abilityKey = KeyCode.Q;
    [SerializeField] KeyCode readLoreKey = KeyCode.X;
    [SerializeField] KeyCode openInventoryKey = KeyCode.G;
    [SerializeField] KeyCode grabHoldableKey = KeyCode.Mouse0;
    [SerializeField] KeyCode buttonPressKey = KeyCode.Mouse1;



    [Space(20)]
    [Header("Mechanisms")]
    [Space(10)]

    float pitch, yaw;
    bool mouseManualControlled = true;


    [Space(10)]
    public Dimension currentPlayerDimension;
    [SerializeField] public Vector3 dimensionalDiffPosition = new Vector3(-10, 0, 0);
    [SerializeField] public Vector3 startingPos = new Vector3(31.3f, 1, 74.5f);
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask isntPlayerMask;
    bool inventoryOpen = false;

    [SerializeField] GameObject listenerObj;

    [Space(20)]
    [Header("Controls")]
    [Space(10)]

    [SerializeField] Vector2 mouseInput;
    [SerializeField,Range(0,10)] float sensitivity = 1;
    [SerializeField] float droneScrollSpeed = 0.2f;
    [SerializeField] float invFadeTime = 0.2f;

    [Space(20)]
    [Header("Movement")]
    [Space(10)]


    [SerializeField] Vector2 moveInput;
    [SerializeField,Min(0.01f)] float speed = 8;
    [SerializeField, Min(0.01f)] float acceleration = 10;

    public GroundType groundType;



    [Header("Visual Effects")]
    [SerializeField] VisualEffect portal;

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
    [SerializeField]CanvasGroup dimensionalUnavailableWarning;
    [SerializeField] float dimensionalUnavailableWarningDuration = 0.7f;

    [SerializeField] GameObject dronePlatformPrefab;
    [SerializeField] BoxCollider droneDraft;
    bool dronePosValid;
    bool dronePlatformDrafting;

    [HideInInspector] public float currentDronePlatformDistance;
    [SerializeField] Vector2 droneDistanceClamps = new Vector2(1, 8);
    DronePlatform currentDronePlatformPrefab;
    [SerializeField] Material[] droneMaterials = new Material[2];
    [SerializeField] MeshRenderer droneMeshRenderer;
    [SerializeField] bool droneMatValid = true;
    [SerializeField] LayerMask playerAndGroundMask;

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
        ForceDimension(0);

        TransformPlayerParent(startingPos);
    }

    public void ResetPlayer()
    {
        ForceDimension(0);

        TransformPlayerParent(startingPos);
        Toggle(true);
        yaw = transform.eulerAngles.y;
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


        //check grounded

        if(!isGrounded) { return; }

        if(!canSwitch) { return; }

        //check valid Position

        RaycastHit hit;
        if (Physics.Raycast(otherPlayer.transform.position, Vector3.down, out hit, Mathf.Infinity,groundMask))
        {
            Debug.Log($"Did Hit Floor {hit.collider.gameObject.name}, distance is {hit.distance}");
            //there is floor somewhere there

            if (!Physics.CheckCapsule(otherPlayer.camTransform.position, otherPlayer.transform.position, 0.5f,groundMask))
            {
                StartCoroutine(dimensionSwitch(to));
            }
            else
            {
                StartCoroutine(ShowDimensionalUnavailability());
            }



        }




    }

    void dimensionJump(Dimension to)
    {
        currentPlayerDimension = to;
        for (int i = 0; i < 2; i++)
        {
            players[i].SwitchTo(i == (int)currentPlayerDimension);
        }
        ChangeGrabParent();
        //ChangeListenerPosition();
        
    }
    bool canSwitch = true;
    public IEnumerator dimensionSwitch(Dimension to)
    {
        canSwitch = false;

        portal.Play();
        foreach(Player p in players)
        {
            p.anim.SetTrigger("DimensionSwitch");
        }



        yield return new WaitForSeconds(0.7f);

        dimensionJump(to);

        yield return new WaitForSeconds (0.7f);

        canSwitch = true;





    }



    void ForceDimension(Dimension to)
    {
        //check valid Position
        currentPlayerDimension = to;
        for (int i = 0; i < 2; i++)
        {
            players[i].SwitchTo(i == (int)currentPlayerDimension);
            //rigidbodys[i].isKinematic = (i != currentPlayerDimension);
        }
        ChangeGrabParent();
       




    }

    IEnumerator ShowDimensionalUnavailability()
    {
        float time = 0;
        while (time < dimensionalUnavailableWarningDuration)
        {
            dimensionalUnavailableWarning.alpha = Mathf.Lerp(1, 0, time / dimensionalUnavailableWarningDuration);
            time += Time.deltaTime;
            yield return null;
        }
        dimensionalUnavailableWarning.alpha = 0;

    }



    //Movement

    bool Vault()
    {
        Vector3 playerLookDirection = currentPlayer.camTransform.forward;
        Vector3 desiredDirection = currentLedge.direction;
        float angle = Vector3.Angle(playerLookDirection, desiredDirection);
        Debug.LogWarning(angle);
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
            rb.velocity = new Vector3(rb.velocity.x, 4, rb.velocity.z);
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
            if (Physics.Raycast(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward, out hit, armReach, isntPlayerMask))
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
    public void RespawnWithoutItem()
    {
        if(currentHeldItem != null)
        {
            Holdable h = currentHeldItem;
            currentHeldItem.transform.parent = null;
            currentHeldItem.ToggleGhost(false);
            currentHeldItem = null;
            h.GetComponent<Respawning>().Respawn();
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
    void ChangeListenerPosition()
    {
        listenerObj.transform.position = currentPlayer.camTransform.position;
    }

    void PressButton()
    {
        RaycastHit hit;
        if (Physics.Raycast(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward, out hit, armReach, isntPlayerMask))
        {
            Debug.DrawRay(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward * hit.distance, Color.cyan,5);
            Debug.Log($"Did Hit Button {hit.collider.gameObject.name}");
            if (hit.collider.GetComponent<Button>() != null)
            {
                lastInteracted = hit.collider.gameObject;
                hit.collider.GetComponent<Button>().Activate();

            }
        }
    }

    public void HackTriggerable()
    {
        currentPlayer.anim.SetTrigger("Hack");
        StartCoroutine(LockOn(lastInteracted.transform.position, 4.125f));
    }

    IEnumerator LockOn(Vector3 pos, float duration)
    {
        float LockLength = 0.25f;
        EnableControls(false);
        float d = duration > LockLength ? LockLength : duration;
        Quaternion LookRotation = Quaternion.LookRotation(pos - currentPlayer.cam.transform.position);
        duration -= d;
        Vector2 pitchYaw = new Vector2(pitch, yaw );

        Quaternion qStart = Quaternion.Euler(pitchYaw);
        Quaternion qGoal = LookRotation;
        Quaternion qCurrent = qStart;



        while (d > 0)
        {
            qCurrent = Quaternion.Lerp(qCurrent, qGoal, 1 - (d / LockLength));
            currentPlayer.transform.rotation = Quaternion.Euler(0, qCurrent.eulerAngles.y, 0);
            currentPlayer.camTransform.localRotation = Quaternion.Euler(qCurrent.eulerAngles.x, 0, 0);
            d -= Time.deltaTime;
            yield return null;
        }
        currentPlayer.transform.rotation = Quaternion.Euler(0, qGoal.eulerAngles.y, 0);
        currentPlayer.camTransform.localRotation = Quaternion.Euler(qGoal.eulerAngles.x, 0, 0);

        d = duration > LockLength ? LockLength : duration;
        duration -= d;
        yield return new WaitForSeconds(duration);
        
        while (d > 0)
        {
            qCurrent = Quaternion.Lerp(qCurrent, qStart, 1 - (d / LockLength));
            currentPlayer.transform.rotation = Quaternion.Euler(0, qCurrent.eulerAngles.y, 0);
            currentPlayer.camTransform.localRotation = Quaternion.Euler(qCurrent.eulerAngles.x, 0, 0);
            d -= Time.deltaTime;
            yield return null;
        }

        currentPlayer.transform.rotation = Quaternion.Euler(0, qStart.eulerAngles.y, 0);
        currentPlayer.camTransform.localRotation = Quaternion.Euler(qStart.eulerAngles.x, 0, 0);









        EnableControls(true);


    }




    //Abilities


    void Repair()
    {
        RaycastHit hit;
        if (Physics.Raycast(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward, out hit, armReach, isntPlayerMask))
        {
            Debug.DrawRay(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward * hit.distance, Color.cyan, 5);
            Debug.Log($"Did Hit Fuse {hit.collider.gameObject.name}");
            if (hit.collider.GetComponent<Repairable>() != null)
            {
                hit.collider.GetComponent<Repairable>().TryRepair(this);
            }
        }
    }

    void ToggleDroneDraft()
    {
        dronePlatformDrafting = !dronePlatformDrafting;
        dronePosValid = false;
        ShowDraft(dronePlatformDrafting);
    }
    void ConfirmDroneDraft()
    {
        if (dronePlatformDrafting)
        {
            dronePlatformDrafting = false;
            NewDronePositionSet();
        }
    }


    void NewDronePositionSet()
    {
        if (dronePosValid)
        {
            currentPlayer.anim.SetTrigger("Point");
            if (currentDronePlatformPrefab != null)
            {
                currentDronePlatformPrefab.StartFlyingUp();
            }

            currentDronePlatformPrefab = Instantiate(dronePlatformPrefab).GetComponent<DronePlatform>();
            currentDronePlatformPrefab.Init(droneDraft.transform.position - 2 * droneDraft.center);

            ShowDraft(false);

        }
        else
        {
            dronePlatformDrafting = true;
        }


    }

    void ShowDraft(bool showDraft)
    {
        dronePlatformDrafting = showDraft;

        droneDraft.gameObject.SetActive(showDraft);

        currentDronePlatformDistance = 5;
        if (showDraft)
        {

        }
        else
        {
            currentDronePlatformDistance = 5;
        }
    }
   

    public void Teleport(Vector3 pos)
    {
        transform.position = pos.PlayerPosToOwner(this);
    }


    //showWheel;


    void ShowWheel()
    {
        UIM.ToggleUI("Remote Interaction", true);
        ShowMouse(true);
        currentPlayer.anim.SetTrigger("HoverHand");

    }

    void HideWheel()
    {
        int selectedNum = GetMouseOverNumber();
        if (selectedNum!=-1)
        {
            CollectedActions[selectedNum].Triggered();
        }

        UIM.ToggleUI("Remote Interaction", false);
        ShowMouse(false);
        currentPlayer.anim.SetTrigger("HoverDown");


    }

    int GetMouseOverNumber()
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

        if (CollectedActions.Count == 0)
        {
            return -1;
        }
        int num = CollectedActions.Count;
        //Debug.LogWarning($"{angle} / {num}");
        int fitCount = Mathf.FloorToInt(angle / (360/ num));
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

        if (dronePlatformDrafting)
        {

            // Calculate the object's size
            Bounds droneBounds = droneDraft.bounds;


            bool validSpot = true;

            RaycastHit wallCheck;
            Vector3 checkBoxPosition;

            if (Physics.Raycast(currentPlayer.cam.transform.position + (currentPlayer.cam.transform.forward * droneDistanceClamps.x),currentPlayer.cam.transform.forward, out wallCheck, currentDronePlatformDistance, groundMask))
            {
                Vector3 directionToPlayer = currentPlayer.cam.transform.position - wallCheck.point;


                if (Physics.BoxCast(wallCheck.point, droneBounds.extents / 2, directionToPlayer.normalized, out RaycastHit boxHit, Quaternion.identity, directionToPlayer.magnitude-droneDistanceClamps.x,groundMask))
                {
                    checkBoxPosition = boxHit.point;
                    Debug.DrawLine(boxHit.point, wallCheck.point);
                }
                else
                {
                    checkBoxPosition = wallCheck.point - currentPlayer.cam.transform.forward * (droneBounds.extents.magnitude + 0.1f); // Adjust 0.1f as needed
                }

            }
            else
            {
                checkBoxPosition = currentPlayer.cam.transform.position + (currentPlayer.cam.transform.forward * currentDronePlatformDistance);
            }
            droneDraft.transform.position = checkBoxPosition;
            checkBoxPosition += droneDraft.center;
            if (Physics.CheckBox(checkBoxPosition, droneBounds.extents / 2, Quaternion.identity, playerAndGroundMask))
            {
                validSpot = false;
            }
            dronePosValid = validSpot;

            if (validSpot)
            {
                if(!droneMatValid)
                {
                    droneMeshRenderer.material = droneMaterials[0];
                    droneMatValid = true;
                }
            }
            else
            {
                if (droneMatValid)
                {
                    droneMeshRenderer.material = droneMaterials[1];
                    droneMatValid = false;
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.R)||transform.position.y<-20)
        {
            ResetPlayer();
        }



        RaycastHit interactable;
        // Check for obstacles between the camera and the grab position
        if (Physics.Raycast(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward, out interactable, armReach, isntPlayerMask))
        {
            if (interactable.transform.TryGetComponent(out Interactable _inter))
            {
                _inter.Seen();
            }
        }


    }

    public void ReadLore()
    {
        RaycastHit hit;
        if (Physics.Raycast(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward, out hit, armReach, isntPlayerMask))
        {
            Debug.DrawRay(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward * hit.distance, Color.cyan, 5);
            Debug.Log($"Did Hit Lore {hit.collider.gameObject.name}");
            if (hit.collider.TryGetComponent(out LoreObject lO))
            {
                loreInv.InteractWithObject(lO);
            }
        }
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            foreach(Player p in players)
            {
                p.anim.SetTrigger("DimensionSwitch");
            }
        }
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

            if(dronePlatformDrafting)
            {
                ConfirmDroneDraft();
            }
            else
            {
                GrabHoldableItem();
            }
        }
        if (Input.GetKeyDown(readLoreKey))
        {
            ReadLore();
        }
        
        if(Input.GetKeyDown(buttonPressKey))
        {
            PressButton();
            Repair();
        }

        if (currentPlayerDimension == Dimension.Cyberpunk)
        {

            if (Input.GetKeyDown(dimensionSwapKey))
            {
                switchDimension(Dimension.Steampunk);
            }
            if (Input.GetKeyDown(abilityKey))
            {
                ShowWheel();
            }
            if (Input.GetKeyUp(abilityKey))
            {
                HideWheel();
            }


        }
        else if (currentPlayerDimension == Dimension.Steampunk)
        {
            if (Input.GetKeyDown(dimensionSwapKey))
            {
                switchDimension(Dimension.Cyberpunk);
            }

            if (Input.GetKeyDown(abilityKey))
            {
                ToggleDroneDraft();
            }
            if(dronePlatformDrafting)
            {
                float scrollInput = Input.mouseScrollDelta.y;
                currentDronePlatformDistance += scrollInput * droneScrollSpeed;
                currentDronePlatformDistance = Mathf.Clamp(currentDronePlatformDistance, droneDistanceClamps.x, droneDistanceClamps.y);
            }
        }
        else
        {

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            loreInv.openInvCanvas();
        }

        if(mouseManualControlled)
        {

            mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y")) * sensitivity;
            pitch += mouseInput.y;
            yaw += mouseInput.x;
            pitch = Mathf.Clamp(pitch, -90, 90);
            foreach (Player p in players)
            {
                p.transform.rotation = Quaternion.Euler(0, yaw, 0);
                p.camTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
            }

        }

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));

        moveInput.Normalize();


        Vector3 movementCalc = Vector3.Lerp(rb.velocity, currentPlayer.transform.forward * speed * moveInput.y + currentPlayer.transform.right * speed * moveInput.x, Time.deltaTime * acceleration);
        movementCalc.y = rb.velocity.y;
        rb.velocity = movementCalc;

    }

    public void EnableControls(bool to)
    {
        mouseManualControlled = to;
        sensitivity = to ? 1 : 0;
        speed = to ? 6 : 0;
    }

}
