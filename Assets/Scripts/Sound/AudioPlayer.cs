using UnityEngine;
using System;

[Serializable]
public class AudioPlayer : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Sound[] soundClips;
    [SerializeField]
    private AudioSource audioSource;
#pragma warning restore 0649

    public void playOneShot(string soundName)
    {
        foreach (Sound sound in soundClips)
        {
            if (sound.name == soundName)
            {
                audioSource.clip = sound.clip;
                audioSource.mute = sound.mute;
                audioSource.loop = sound.loop;
                audioSource.volume = sound.volume;
                audioSource.spatialBlend = sound.spacialBlend;
                audioSource.PlayOneShot(audioSource.clip);
                break;
            }
        }
    }
}
