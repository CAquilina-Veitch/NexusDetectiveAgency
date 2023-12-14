using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FootstepGuy : MonoBehaviour
{
    public enum GroundType { normal, concrete, metal, wood, dirt }

    float StepRandom;
    Vector3 PrevPos;
    bool PlayerTouchingGround
    {
        get
        {
            if (isDetective)
            {
                return true;
            }
            else
            {
                return pC.isGrounded;
            }
        }
    }

    public bool isDetective;
    public PlayerController pC;
    float DistanceTravelled;


    void Start() // This method, and everyting inside of it, is perfomred on the very first frame that is run when we start the game.
    {
        StepRandom = Random.Range(0f, 0.5f);        // 'Step Random' now holds a value. It could be anywhere between 0 and 0.5.
        PrevPos = transform.position;               // 'PrevPos' now holds the location that the player is starting at as co-ordinates (x, y, z).
    }


    void PlayFootstep() // When this method is performed, our footsteps event in FMOD will be told to play.
    {
        if (PlayerTouchingGround)                                                                                    // First we check to see the player is touching the ground.
        {
            FMOD.Studio.EventInstance Footstep = FMODUnity.RuntimeManager.CreateInstance("event:/Footsteps/EventFootstepNormal");        // If they are, we create an FMOD event instance. We use the event path inside the 'FootstepsEventPath' variable to find the event we want to play.
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(Footstep, transform, GetComponent<Rigidbody>());     // Next that event instance is told to play at the location that our player is currently at.
            Footstep.setParameterByName("FloorType", (int)MaterialCheck());                                     // Before the event is played, we set the Material Parameter to match the value of the 'F_MaterialValue' variable.
            Footstep.start();                                                                                        // We then play a footstep!.
            Footstep.release();                                                                                      // We also set our event instance to release straight after we tell it to play, so that the instance is released once the event had finished playing.
        }
    }


    RaycastHit recentHit;

    GroundType currentGroundType;
    GroundType MaterialCheck() // This method when performed will find out what material our player is currenly on top of and will update the value of 'F_MaterialValue' accordingly, to represent that value.
    {
        if (isDetective)
        {
            return GroundType.wood;
        }
        else
        {
            RaycastHit recentHit = pC.groundHit;
            string tag = recentHit.collider.tag;
            Debug.LogWarning("steping on " + tag);
            if (tag == "Untagged")
            {
                return GroundType.normal;
            }
            else if (tag == "Concrete")
            {
                return GroundType.concrete;
            }
            else if (tag == "Metal")
            {
                return GroundType.metal;
            }
            else if (tag == "Wood")
            {
                return GroundType.wood;
            }
            else if (tag == "Dirt")
            {
                return GroundType.dirt;
            }
            else
            {
                return GroundType.normal;
            }

        }
    }

    float TimeTakenSinceStep;
    float StepDistance;
    private void FixedUpdate()
    {
        TimeTakenSinceStep += Time.deltaTime;                                // This adds whatever value Time.deltaTime is at to the 'TimeTakenToStep' float. Essentially tunring 'TimeTakenToStep' into a running timer that starts at 0 seconds. Remeber that 'Time.deltaTime' counts how long in seconds the last frame lasted for. 
        DistanceTravelled += (transform.position - PrevPos).magnitude;       // Every frame, the co-ordinates that the player is currently at is reduced by the value of the co-ordinates they were at during the previous frame. This gives us the length between the two points as a new set of co-ordinates (AKA a vector). That length is then tunred into a single number by '.magnitude' and then finally added onto the value of the DistanceTravelled float. Now we know how far the player has travlled!
        if (DistanceTravelled >= StepDistance + StepRandom)                  // If the distance the player has travlled is greater than or equal to the StepDistance plus the StepRandom, then we can perform our methods.
        {                                             // The MaterialCheck method is perfomred. This checks for a material value and updates the 'F_MaterialValue' variable with for our 'PlayFootstep()' method to use.
            PlayFootstep();                                                  // The PlayFootstep method is performed and a footstep audio file from FMOD is played!
            StepRandom = Random.Range(0f, 0.5f);                             // Now that our footstep has been played, this will reset 'StepRandom' and give it a new random value between 0 and 0.5, in order to make the distance the player has to travel to hear a footstep different from what it previously was.
            DistanceTravelled = 0f;                                          // Since the player has just taken a step, we need to set the 'DistanceTravelled' float back to 0.
        }
        PrevPos = transform.position;
    }








}
