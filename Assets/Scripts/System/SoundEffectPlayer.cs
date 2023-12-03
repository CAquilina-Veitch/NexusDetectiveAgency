using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public string id;
    public bool playOnStart;

    public void playSound()
    {
        GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SoundEffectManager>().PlaySoundEffect(id,transform);
    }
    public void playSound(float delay)
    {
        GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SoundEffectManager>().PlaySoundEffectWithDelay(id,delay);
    }
    private void Start()
    {
        if(playOnStart)
        {
            playSound();
        }
    }

}
