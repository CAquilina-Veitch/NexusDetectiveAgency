using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;

//[RequireComponent(typeof(StudioEventEmitter))]
public class SoundEmitter : MonoBehaviour
{
    [SerializeField] EventInstance soundInst;
    [SerializeField] string eventString;

    [SerializeField] bool PlayOnStart;

    //StudioEventEmitter objSound;
    public void Start()
    {
        //objSound = GetComponent<StudioEventEmitter>();
        soundInst = RuntimeManager.CreateInstance(eventString);
        if(PlayOnStart == true)
        {
            StartSound();
        }
    }

    private void Update()
    {
        RuntimeManager.AttachInstanceToGameObject(soundInst, transform);
        RuntimeManager.AttachInstanceToGameObject(soundInst, GetComponent<Transform>(), GetComponent<Rigidbody>());
    }

    public void StartSound()
    {
        soundInst.start();
    }
    public void StopSound()
    {
        soundInst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
