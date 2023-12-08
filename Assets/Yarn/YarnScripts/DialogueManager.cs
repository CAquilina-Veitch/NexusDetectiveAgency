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
    
    private void Awake()
    {
        variableStorage = FindObjectOfType<InMemoryVariableStorage>();              //Finds variables within Yarn
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();             //Finds Yarn's Dialogue Runner
        melanieVoice = FMODUnity.RuntimeManager.CreateInstance(melanieVoicePath);   //Finds Melanie's lines
        maxVoice = FMODUnity.RuntimeManager.CreateInstance(maxVoicePath);           //Finds Max's lines
    }

    [YarnCommand("playvoicemel")]                                                   //Creates a custom Command in Yarn
    public void SetMelanieID()                                                      //Begins when Yarn displays a line's subtitles
    {
        float voiceID;
        variableStorage.TryGetValue("$audioMelanieNumber", out voiceID);            //Retrieves Yarn's written voice line as a number
        melanieVoice.setParameterByName("MelanieLine", voiceID);                    //Sends that number to FMOD to determine a voice line
        melanieVoice.start();                                                       //Play sound in FMOD
        Debug.Log($"Melanie Animation: {voiceID}");                                 //Play animation here
    }
    [YarnCommand("playvoicemax")]
    public void SetMaxID()
    {
        float voiceID;
        variableStorage.TryGetValue("$audioMaxNumber", out voiceID);
        maxVoice.setParameterByName("MaxLine", voiceID);
        maxVoice.start();
    }
}
