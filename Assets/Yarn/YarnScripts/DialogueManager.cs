using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private DialogueRunner dialogueRunner;             //Dialogue Runner is Yarn's script management
    private InMemoryVariableStorage variableStorage;   //How variables are stored within Yarn
    [SerializeField] private LoreInventory loreInventory;
    [SerializeField] LineView lineView;
    [SerializeField] MelSpin melSpin;

    public int documentsFound;
    public bool mainDocFound;
    public bool hologramEffectActive;
    private float lineDuration;

    FMOD.Studio.EventInstance melanieVoice;            //FMOD instance of Melanie's audio
    FMOD.Studio.EventInstance maxVoice;                //FMOD instance of Max's audio
    public string melanieVoicePath;                    //Path to Melanie's voice lines in FMOD
    public string maxVoicePath;                        //Path to Max's voice lines in FMOD

    [SerializeField] UnityEvent EndDialogue;
    [SerializeField] UnityEvent EndDialogueTemp;

    public Transform melanieTran;
    public Transform maxTran;

    public Animator melanieAnim;
    public Animator maxAnim;
    
    private void Start()
    {
        variableStorage = FindObjectOfType<InMemoryVariableStorage>();              //Finds variables within Yarn
        //loreInventory = FindObjectOfType<LoreInventory>();
        //dialogueRunner = FindObjectOfType<DialogueRunner>();             //Finds Yarn's Dialogue Runner
        dialogueRunner = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<DialogueRunner>();

        //lineView = FindObjectOfType<LineView>();
        lineView = GameObject.FindGameObjectWithTag("Dialogue").transform.GetChild(0).GetChild(0).GetComponent<LineView>();

        melanieVoice = RuntimeManager.CreateInstance(melanieVoicePath);   //Finds Melanie's lines
        maxVoice = RuntimeManager.CreateInstance(maxVoicePath);           //Finds Max's lines


    }

    private void Update()
    {
        RuntimeManager.AttachInstanceToGameObject(melanieVoice, melanieTran);
        RuntimeManager.AttachInstanceToGameObject(maxVoice, maxTran);

    }

     

    public void StartMelConvo()
    {
        melanieAnim.SetTrigger("Start Dialogue");
        melSpin.Spin();
        StartCoroutine(DelayDialogueStart());
    }
    IEnumerator DelayDialogueStart()
    {
        yield return new WaitForSeconds(11.25f); //11.25
        dialogueRunner.StartDialogue("MelanieDialogue");
    }

    [YarnCommand("playvoicemel")]                                                   //Creates a custom Command in Yarn
    public void SetMelanieID()                                                      //Begins when Yarn displays a line's subtitles
    {
        float voiceMelID;
        variableStorage.TryGetValue("$audioMelanieNumber", out voiceMelID);            //Retrieves Yarn's written voice line as a number
        variableStorage.TryGetValue("$AudioCutOut", out bool cut);
        variableStorage.TryGetValue("$LineDuration", out float duration);
        lineDuration = duration;

        if (!cut)
        {
            melanieVoice.setParameterByName("MelanieLine", voiceMelID);
            melanieVoice.start();
        }
        else
        {
            voiceMelID += 99;
        }
        //if (hologramEffectActive == true)
        //{
        //    //Do hologram effect
        //}
        melanieAnim.SetInteger("Dialogue Stage", (int)voiceMelID);
        melanieAnim.SetTrigger("Dialogue Trigger");


        StartCoroutine(GoNextDialogue());
        //Debug.Log($"Melanie Animation: {voiceID}");                                 //Play animation here
    }
    [YarnCommand("playvoicemax")]
    public void SetMaxID()
    {
        float voiceMaxID;
        variableStorage.TryGetValue("$audioMaxNumber", out voiceMaxID);
        maxVoice.setParameterByName("MaxToMel", voiceMaxID);
        variableStorage.TryGetValue("$LineDuration", out float duration);
        lineDuration = duration;

        maxVoice.start();

        StartCoroutine(GoNextDialogue());
    }
    //[YarnCommand("loredocsfound")]
    //public void LoreDocsFound()
    //{
    //    documentsFound = GameObject.FindGameObjectWithTag("LoreInventory").GetComponent< LoreInventory>().collectedLore();
    //    mainDocFound = GameObject.FindGameObjectWithTag("LoreInventory").GetComponent<LoreInventory>().isEnough();
    //    variableStorage.SetValue("$documentsFound", documentsFound);
    //    variableStorage.SetValue("$FetchDocFound", mainDocFound);
    //}
    [YarnCommand("melaniestop")]
    public void MelanieStop()
    {
        EndDialogueTemp.Invoke();
    }
    [YarnCommand("melaniesuccess")]
    public void OpenFinalDoor()
    {
        EndDialogue.Invoke();
    }
    [YarnCommand("autonext")]
    public void AutoNext()
    {
        StartCoroutine(GoNextDialogue());
    }
    [YarnCommand("holoeff")]
    public void HologramOff()
    {
        Debug.Log("play hologram");
    }


    IEnumerator GoNextDialogue()
    {
        yield return new WaitForSeconds(lineDuration);
        lineView.GetComponent<LineView>().UserRequestedViewAdvancement();
    }

}
