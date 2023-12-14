using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        //dialogueRunner = FindObjectOfType<DialogueRunner>();                      //Finds Yarn's Dialogue Runner
        dialogueRunner = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<DialogueRunner>();

        //lineView = FindObjectOfType<LineView>();
        lineView = GameObject.FindGameObjectWithTag("Dialogue").transform.GetChild(0).GetChild(0).GetComponent<LineView>();

        melanieVoice = RuntimeManager.CreateInstance(melanieVoicePath);             //Finds Melanie's lines
        maxVoice = RuntimeManager.CreateInstance(maxVoicePath);                     //Finds Max's lines

        MelHologramOn(true);
    }

    private void Update()
    {
        RuntimeManager.AttachInstanceToGameObject(melanieVoice, melanieTran);
        RuntimeManager.AttachInstanceToGameObject(maxVoice, maxTran);
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            HologramOff();
        }
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

    IEnumerator GoNextDialogue()
    {
        yield return new WaitForSeconds(lineDuration);
        lineView.GetComponent<LineView>().UserRequestedViewAdvancement();
    }



    [YarnCommand("playvoicemel")]
    public void SetMelanieID()
    {
        variableStorage.TryGetValue("$audioMelanieNumber", out float voiceMelID);
        variableStorage.TryGetValue("$AudioCutOut", out bool cut);
        variableStorage.TryGetValue("$LineDuration", out float duration);

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

        lineDuration = duration;
        StartCoroutine(GoNextDialogue());
    }

    [YarnCommand("playvoicemax")]
    public void SetMaxID()
    {

        variableStorage.TryGetValue("$audioMaxNumber", out float voiceMaxID);
        variableStorage.TryGetValue("$LineDuration", out float duration);
        variableStorage.TryGetValue("$AudioCutOut", out bool maxTyping);
        maxVoice.setParameterByName("MaxToMel", voiceMaxID);

        if(maxTyping == true)
        {
            //maxAnim.SetInteger("Dialogue Stage", (int)voiceMaxID);                //Max Animated hand typing here
            //maxAnim.SetTrigger("Dialogue Trigger");
        }

        maxVoice.start();
        lineDuration = duration;
        StartCoroutine(GoNextDialogue());
    }

    [YarnCommand("loredocsfound")]
    public void LoreDocsFound()
    {

        mainDocFound = GameObject.FindGameObjectWithTag("LoreInventory").GetComponent<LoreInventory>().isEnough();
        variableStorage.SetValue("$FetchDocFound", mainDocFound);
    }

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
        StartCoroutine(HologramAnim());
    }
    IEnumerator HologramAnim()
    {
        MelHologramOn(false);
        yield return new WaitForSeconds(0.7f/2);
        MelHologramOn(true);
        yield return new WaitForSeconds(0.2f / 2);
        MelHologramOn(false);        
        yield return new WaitForSeconds(0.1f / 2);
        MelHologramOn(true);
        yield return new WaitForSeconds(0.3f / 2);
        MelHologramOn(false);        
        yield return new WaitForSeconds(0.2f / 2);
        MelHologramOn(true);
        yield return new WaitForSeconds(1.3f / 2);
        MelHologramOn(false);


    }

    public Material[] mats = new Material[2];
    bool isHologram = false;
    public void MelHologramOn(bool to)
    {
        if (isHologram == to)
        {
            return;
        }
        isHologram = to;

        SkinnedMeshRenderer[] mRs = melanieTran.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer mr in mRs)
        {
            if (mr.gameObject.activeInHierarchy)
            {
                if (mr.material == to ? mats[0] : mats[1])
                {
                    mr.material = to ? mats[1] : mats[0];
                }
            }
        }
    }


}
