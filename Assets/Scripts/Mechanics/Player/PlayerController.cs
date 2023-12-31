using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.VFX;
using FMOD.Studio;
using FMODUnity;

public enum Dimension { Cyberpunk, Steampunk, Noir}


public class PlayerController : MonoBehaviour
{
    GameObject lastInteracted;
    [Space(20)]
    [Header("Dependencies")]
    [Space(10)]

    [SerializeField] Rigidbody rb;

    [SerializeField] LoreInventory loreInv;
    [SerializeField] Player[] players = new Player[2];


    [SerializeField] GameObject tinyPlatform;
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
    //[SerializeField] StudioListener listenion;

    [Space(20)]
    [Header("Controls")]
    [Space(10)]

    [SerializeField] Vector2 mouseInput;
    [SerializeField,Range(0,10)] public float sensitivity = 1;
    [SerializeField] float realSens = 1;
    [SerializeField] float droneScrollSpeed = 0.2f;
    [SerializeField] float invFadeTime = 0.2f;

    [Space(20)]
    [Header("Movement")]
    [Space(10)]


    [SerializeField] Vector2 moveInput;
    [SerializeField,Min(0.01f)] float speed = 8;
    [SerializeField, Min(0.01f)] float acceleration = 10;



    [Header("Visual Effects")]
    [SerializeField] VisualEffect portal;

    [SerializeField] Transform vfxParent;

    private void Start()
    {
        loreInv = GameObject.FindGameObjectWithTag("LoreInventory").GetComponent<LoreInventory>();
        loreInv.Set(this);
    }


    public RaycastHit groundHit;
    public bool isGrounded
    {
        get
        {
            Debug.DrawRay(currentPlayer.transform.position, Vector3.down * jumpCheckLength, Color.cyan, 5.0f);
            if (Physics.Raycast(currentPlayer.transform.position,Vector3.down, out groundHit,jumpCheckLength, groundMask))
            {
                
                return true;
            }
            else
            {
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
    public List<DronePlatform> currentDronePlatformPrefabs = new List<DronePlatform>();
    [SerializeField] Material[] droneMaterials = new Material[2];
    [SerializeField] MeshRenderer droneMeshRenderer;
    [SerializeField] bool droneMatValid = true;
    [SerializeField] LayerMask playerAndGroundMask;

    public bool hasFuse;


    [Space(10)]

    public TriggerableObject hackedTrigger;
    //public List<TriggerableObject> CollectedActions = new List<TriggerableObject>();


    [Space(20)]
    [Header("Audio")]
    [Space(10)]
    [SerializeField] SoundEmitter soundJump;
    [SerializeField] SoundEmitter soundHurt;
    [SerializeField] SoundEmitter soundDimswap;
    [SerializeField] SoundEmitter soundMoveplat;
    [SerializeField] EventInstance soundMusic;
    //"".StartSound();


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

    public void Ready()
    {
        rb.useGravity = true;
        Toggle(true);
    }

    private void Awake()
    {
        ForceDimension(0);
        ChangeListenerPosition();
        soundMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/EventSimulMusic");
        soundMusic.setParameterByName("WhatDimension", (int) currentPlayerDimension);
        soundMusic.start();
        TransformPlayerParent(startingPos);
    }

    public void changeStartPosition()
    {
        startingPos = transform.position;
    }
    public void ResetPlayer()
    {
        ForceDimension(0);

        soundMusic.setParameterByName("WhatDimension", (int)currentPlayerDimension);
        soundHurt.StartSound();
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
        else
        {
            StartCoroutine(ShowDimensionalUnavailability());
        }




    }

    void dimensionJump(Dimension to)
    {
        currentPlayerDimension = to;
        for (int i = 0; i < 2; i++)
        {
            players[i].SwitchTo(i == (int)currentPlayerDimension);
            if(players[i].GetComponent<Weight>().currentPlate != null)
            {
                Weight w = players[i].GetComponent<Weight>();
                if (w.currentPlate.weights.Contains(w))
                {
                    w.currentPlate.weights.Remove(w);
                }
            }
        }
        soundMusic.setParameterByName("WhatDimension", (int)to);
        ChangeGrabParent();
        ChangeVFXParent();
        ChangeListenerPosition();

    }
    bool canSwitch = true;
    public IEnumerator dimensionSwitch(Dimension to)
    {
        Debug.Log("Shompf");
        canSwitch = false;
        ToggleDroneDraft(false);


        soundDimswap.StartSound();


        portal.Play();
        foreach(Player p in players)
        {
            p.anim.SetTrigger("DimensionSwitch");
        }


        EnableControls(false);
        yield return new WaitForSeconds(0.7f);

        dimensionJump(to);

        yield return new WaitForSeconds (0.7f);
        EnableControls(true);
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
    public float jumpHeight = 2;
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
            rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
            soundJump.StartSound();
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
    void ChangeVFXParent()
    {
        vfxParent.transform.parent = currentPlayer.handCam.transform;
        vfxParent.transform.position = currentPlayer.handCam.transform.position;
        vfxParent.transform.localRotation = Quaternion.identity;
    }
    void ChangeListenerPosition()
    {
        listenerObj.transform.position = currentPlayer.camTransform.position;
        listenerObj.transform.parent = currentPlayer.cam.transform;
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
        StartCoroutine(LockOn(lastInteracted.transform.position, 4.125f/2));
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
            if (hit.collider.GetComponent<Repairable>() != null)
            {
                hit.collider.GetComponent<Repairable>().TryRepair(this);
            }
        }
    }

    public void ToggleDroneDraft()
    {
        Debug.Log(!dronePlatformDrafting);
        dronePlatformDrafting = !dronePlatformDrafting;
        dronePosValid = false;
        ShowDraft(dronePlatformDrafting);
    }    
    public void ToggleDroneDraft(bool to)
    {
        Debug.Log(to);
        dronePlatformDrafting = to;
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
            

            DronePlatform currentDronePlatformPrefab = Instantiate(dronePlatformPrefab).GetComponent<DronePlatform>();
            currentDronePlatformPrefab.Init(droneDraft.transform.position - 2 * droneDraft.center);
            currentDronePlatformPrefabs.Add(currentDronePlatformPrefab);
            ShowDraft(false);

            if(currentDronePlatformPrefabs.Count>2)
            {
                Debug.Log("I am greater than 2");
                if (currentDronePlatformPrefabs[0] != null)
                {
                    currentDronePlatformPrefabs[0].StartFlyingUp();
                }
                currentDronePlatformPrefabs.RemoveAt(0);
                
            }


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
        StartCoroutine(die(pos));
    }

    IEnumerator die(Vector3 pos)
    {
        GameObject.FindGameObjectWithTag("Swirler").GetComponent<swirl>().Swirl();
        yield return new WaitForSeconds(0.5f);
        GameObject.FindGameObjectWithTag("Fadeblack").GetComponent<Fadeblack>().Fade(true,0.2f);
        yield return new WaitForSeconds(0.2f);
        transform.position = pos.PlayerPosToOwner(this);
        RespawnWithoutItem();
        GameObject.FindGameObjectWithTag("Fadeblack").GetComponent<Fadeblack>().Fade(false, 0.2f);
        GameObject.FindGameObjectWithTag("Fadeblack").GetComponent<Fadeblack>().camOff();
        


    }

    //showWheel;


    void ShowWheel()
    {
        if (hackedTrigger == null)
        {

        }
        else
        {
            currentPlayer.anim.SetTrigger("HoverHand");
            StartCoroutine(showTinyPlatform());
        }

    }
    IEnumerator showTinyPlatform()
    {
        yield return new WaitForSeconds(1.07f);
        tinyPlatform.SetActive(true);
        soundMoveplat.StartSound();
        hackedTrigger.Triggered();
        yield return new WaitForSeconds(0.99f);
        tinyPlatform.SetActive(false);
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

        if(Input.GetKeyDown(KeyCode.R)||transform.position.y<-10)
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

    LoreObject last;
    public void ReadLore()
    {
        if (loreInv.singleIsOpen)
        {
            EnableControls(!loreInv.InteractWithObject(last));
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward, out hit, armReach, isntPlayerMask))
            {
                Debug.DrawRay(currentPlayer.cam.transform.position, currentPlayer.cam.transform.forward * hit.distance, Color.cyan, 5);
                Debug.Log($"Did Hit Lore {hit.collider.gameObject.name}");
                if (hit.collider.TryGetComponent(out LoreObject lO))
                {
                    last = lO;
                    EnableControls(!loreInv.InteractWithObject(lO));

                }
            }
        }
       
    }



    void Update()
    {
        if (Input.GetKeyDown(readLoreKey))
        {
            ReadLore();
        }

        if (Input.GetKeyDown(openInventoryKey))
        {
            loreInv.openInvCanvas();
        }
        if (mouseManualControlled)
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
            if (Input.GetKeyDown(grabHoldableKey))
            {

                if (dronePlatformDrafting)
                {
                    ConfirmDroneDraft();
                }
                else
                {
                    GrabHoldableItem();
                }
            }


            if (Input.GetKeyDown(buttonPressKey))
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


            }
            else if (currentPlayerDimension == Dimension.Steampunk)
            {

                if (Input.GetKeyDown(dimensionSwapKey))
                {
                    switchDimension(Dimension.Cyberpunk);
                }
                if (Input.GetKeyDown(abilityKey))
                {
                    ToggleDroneDraft(false);
                }
                if (dronePlatformDrafting)
                {
                    float scrollInput = Input.mouseScrollDelta.y;
                    currentDronePlatformDistance += scrollInput * droneScrollSpeed;
                    currentDronePlatformDistance = Mathf.Clamp(currentDronePlatformDistance, droneDistanceClamps.x, droneDistanceClamps.y);
                }
            }
            mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y")) * realSens;
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
        Debug.Log($"Enabling {to}");
        mouseManualControlled = to;
        realSens = to ? sensitivity : 0;
        speed = to ? 8 : 0;
    }

    public void activateCameras(bool to)
    {
        currentPlayer.cam.enabled = to;
        currentPlayer.handCam.enabled = to;
        EnableControls(to);
    }


}
