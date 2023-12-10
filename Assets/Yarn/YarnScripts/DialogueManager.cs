using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private DialogueRunner dialogueRunner;             //Dialogue Runner is Yarn's script management
    private InMemoryVariableStorage variableStorage;   //How variables are stored within Yarn

    FMOD.Studio.EventInstance melanieVoice;            //FMOD instance of Melanie's audio
    FMOD.Studio.EventInstance maxVoice;                //FMOD instance of Max's audio
    public string melanieVoicePath;                    //Path to Melanie's voice lines in FMOD
    public string maxVoicePath;                        //Path to Max's voice lines in FMOD

    public Transform melanieTran;
    public Transform maxTran;

    public Animator melanieAnim;
    
    private void Awake()
    {
        variableStorage = FindObjectOfType<InMemoryVariableStorage>();              //Finds variables within Yarn
        dialogueRunner = FindObjectOfType<DialogueRunner>();             //Finds Yarn's Dialogue Runner
        melanieVoice = RuntimeManager.CreateInstance(melanieVoicePath);   //Finds Melanie's lines
        maxVoice = RuntimeManager.CreateInstance(maxVoicePath);           //Finds Max's lines


    }

    private void Update()
    {
        RuntimeManager.AttachInstanceToGameObject(melanieVoice, melanieTran);
        RuntimeManager.AttachInstanceToGameObject(maxVoice, maxTran);

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            StartMelConvo();
        }
    }

     

    public void StartMelConvo()
    {
        melanieAnim.SetTrigger("Start Dialogue");
        StartCoroutine(DelayDialogueStart());
        
    }
    IEnumerator DelayDialogueStart()
    {
        yield return new WaitForSeconds(11.25f);
        dialogueRunner.StartDialogue("MelanieDialogue");
    }

    [YarnCommand("playvoicemel")]                                                   //Creates a custom Command in Yarn
    public void SetMelanieID()                                                      //Begins when Yarn displays a line's subtitles
    {
        float voiceMelID;
        variableStorage.TryGetValue("$audioMelanieNumber", out voiceMelID);            //Retrieves Yarn's written voice line as a number
        variableStorage.TryGetValue("$AudioCutOut", out bool cut);
        if (!cut)
        {
            melanieVoice.setParameterByName("MelanieLine", voiceMelID);
            melanieVoice.start();
        }
        else
        {
            voiceMelID += 99;
        }
        melanieAnim.SetInteger("Dialogue Stage", (int)voiceMelID);
        melanieAnim.SetTrigger("Dialogue Trigger");

        //Debug.Log($"Melanie Animation: {voiceID}");                                 //Play animation here
    }
    [YarnCommand("playvoicemax")]
    public void SetMaxID()
    {
        float voiceMaxID;
        variableStorage.TryGetValue("$audioMaxNumber", out voiceMaxID);
        maxVoice.setParameterByName("MaxToMel", voiceMaxID);
        maxVoice.start();
    }
    [YarnCommand("melaniesuccess")]
    public void OpenFinalDoor()
    {
        //Melanie convo successfully finished, opens last door
    }
}
