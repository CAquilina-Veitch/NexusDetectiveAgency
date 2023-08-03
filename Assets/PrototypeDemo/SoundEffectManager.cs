using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SoundEffectData
{
    public string name;
    public AudioClip clip;
    [SerializeField] float relativeVolume;
    public float Volume
    {
        get
        {
            return relativeVolume;
        }
        set
        {
            relativeVolume = Mathf.Min(value, 0);
        }
    }
    public bool isLooping;
}


public class SoundEffectManager : MonoBehaviour
{
    public List<SoundEffectData> sfxd;
    public GameObject sfxPrefab;
    public float SFXVolume = 1;

    public void SetVolume(float to)
    {
        SFXVolume = to;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Sound"))
        {
            g.GetComponent<SoundEffect>().ChangeVolume(SFXVolume);
        }
    }
    public void PlaySoundEffect(string id)////////plays as player
    {
        SoundEffectData currentSfx;
        if (!sfxd.Exists(x => x.name == id))
        {
            return;
        }
        currentSfx = sfxd.Find(x => x.name == id);
        GameObject obj = Instantiate(sfxPrefab, transform);
        obj.name = $"SFX - {currentSfx.name}";
        obj.GetComponent<AudioSource>().volume = currentSfx.Volume * SFXVolume;
        obj.GetComponent<AudioSource>().clip = currentSfx.clip;
        obj.GetComponent<SoundEffect>().Init(currentSfx);
    }
    public void PlaySoundEffect(string id,Transform atPosition)
    {
        SoundEffectData currentSfx;
        if (!sfxd.Exists(x => x.name == id))
        {
            return;
        }
        currentSfx = sfxd.Find(x => x.name == id);
        GameObject obj = Instantiate(sfxPrefab, atPosition.position, Quaternion.identity,transform);
        obj.name = $"SFX - {currentSfx.name}";
        obj.GetComponent<AudioSource>().volume = currentSfx.Volume * SFXVolume;
        obj.GetComponent<AudioSource>().clip = currentSfx.clip;
        obj.GetComponent<AudioSource>().spatialBlend = 1;
        obj.GetComponent<SoundEffect>().Init(currentSfx);
    }
    public void PlaySoundEffectWithDelay(string id, float delay)
    {
        StartCoroutine(DelayedSFX(id, delay));
    }
    IEnumerator DelayedSFX(string id, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySoundEffect(id);

    }

}
