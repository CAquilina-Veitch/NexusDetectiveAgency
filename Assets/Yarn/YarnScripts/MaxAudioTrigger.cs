using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using Yarn.Unity;

public class MaxAudioTrigger : MonoBehaviour
{
    [SerializeField] EventInstance maxDiaSolo;
    [SerializeField] string maxDiaPath = "event:/Dialogue/EventMaxPuzzles";
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] LineView lineView;
    public int maxLineNumber;

    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        maxDiaSolo = FMODUnity.RuntimeManager.CreateInstance(maxDiaPath);
    }

    public void AudioTriggerEnter()
    {
        dialogueRunner.StartDialogue($"{maxLineNumber}");
        maxDiaSolo.setParameterByName("MaxPuzzles", maxLineNumber);
        maxDiaSolo.start();
        StartCoroutine(NextDialogue());
    }
    IEnumerator NextDialogue()
    {
        yield return new WaitForSeconds(4f);
        lineView.GetComponent<LineView>().UserRequestedViewAdvancement();
    }
}
