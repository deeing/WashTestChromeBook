using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// using MyBox;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public bool mute;
    [Range(0f,1f)]
    public float volume;
    public bool loop;
    [Range(0f, 1f)]
    public float spacialBlend;

    // Only get/set - public - private set/pubic get or vice versa
    // public get/set but not in inspector
    [HideInInspector]
    public AudioSource audioSource;

    public Sound(string name, AudioClip clip, bool mute, float volume, bool loop, float spacialBlend) {
        this.name = name;
        this.clip = clip;
        this.mute = mute;
        this.volume = volume;
        this.loop = loop;
        this.spacialBlend = spacialBlend;
    }
}
