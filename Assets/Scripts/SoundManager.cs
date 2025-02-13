using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private List <AudioSource> allSounds = new List <AudioSource>();
    void Start()
    {
        var childrenComponents = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource audioSource in childrenComponents)
        {
            allSounds.Add(audioSource);
        }
    }

    public void MuteAll()
    {
        foreach (AudioSource audioSource in allSounds)
        {
            audioSource.mute = true;
        }
    }

    public void UnMuteAll()
    {
        foreach (AudioSource audioSource in allSounds)
        {
            audioSource.mute = false;
        }
    }

    public void ToggleMute()
    {
        foreach (AudioSource audioSource in allSounds)
        {
            audioSource.mute = !audioSource.mute;
        }
    }
}
