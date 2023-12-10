using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundSettingsFMOD : MonoBehaviour
{
    FMOD.Studio.Bus masterFMOD;
    FMOD.Studio.Bus sfxFMOD;
    FMOD.Studio.Bus musicFMOD;
    FMOD.Studio.Bus dialogueFMOD;

    public string masterRoute = "bus:/SoundMaster";
    public string sfxRoute = "bus:/SoundMaster/SFX";
    public string musicRoute = "bus:/SoundMaster/Music";
    public string dialogueRoute = "bus:/SoundMaster/Dialogue";

    float masterVol = 1f;
    float sfxVol = 1f;
    float musicVol = 1f;
    float dialogueVol = 1f;

    private void Awake()
    {
        masterFMOD = RuntimeManager.GetBus(masterRoute);
        sfxFMOD = RuntimeManager.GetBus(sfxRoute);
        musicFMOD = RuntimeManager.GetBus(musicRoute);
        dialogueFMOD = RuntimeManager.GetBus(dialogueRoute);
    }

    private void Update()
    {
        masterFMOD.setVolume(masterVol);
        sfxFMOD.setVolume(sfxVol);
        musicFMOD.setVolume(musicVol);
        dialogueFMOD.setVolume(dialogueVol);
    }

    public void MasterVolumeLevel (float newMasterVol)
    {
        masterVol = newMasterVol;
    }
    public void SfxVolumeLevel(float newSfxVol)
    {
        sfxVol = newSfxVol;
    }
    public void MusicVolumeLevel(float newMusicVol)
    {
        musicVol = newMusicVol;
    }
    public void DialogueVolumeLevel(float newDialogueVol)
    {
        dialogueVol = newDialogueVol;
    }
}
