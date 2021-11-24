using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

#pragma warning disable 0649
    public static AudioManager instance { get; private set; }
    [SerializeField]
    private Sound[] soundClips;
#pragma warning restore 0649

    [SerializeField]
    private float fadeTime = .5f;

    private float fadeInterval = .05f;
    private float timeFaded = 0f;
    private WaitForSeconds fadeIntervalWait;
    private Coroutine fadeOutCoroutine;
    private Coroutine fadeInCoroutine;

    private Sound backgroundMusic;

    private void Awake() {
        initInstance();
    }

    private void Start() {
        fadeIntervalWait = new WaitForSeconds(fadeInterval);
    }

    private void initInstance() {
        if (instance == null) {
            instance = this;
            setup();
        }
        else {
            Destroy(this);
        }
    }

    private void setup() {
        foreach (Sound sound in soundClips) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.mute = sound.mute;
            source.loop = sound.loop;
            source.volume = sound.volume;
            // TODO: Add a priorty to the sound class

            sound.audioSource = source;
        }
    }

    private Sound findSound(string soundName)
    {
        foreach (Sound sound in soundClips)
        {
            if (sound.name == soundName)
            {
                return sound;
            }
        }

        Debug.LogWarning("Could not find sound " + soundName);
        return null;
    }

    // TODO: Maybe convert private dictionary to make more effecient
    public void playSound(string soundName) {
        Sound sound = findSound(soundName);
        if (sound != null)
        {
            sound.audioSource.Play();
        }
    }

    public void PlayOneShot(string soundName)
    {
        Sound sound = findSound(soundName);
        if (sound != null)
        {
            sound.audioSource.PlayOneShot(sound.audioSource.clip);
        }
    }

    public void playBackground(string soundName)
    {
        Sound sound = findSound(soundName);
        if (sound != null && sound != backgroundMusic)
        {
            if(backgroundMusic != null)
            {
                fadeOut(backgroundMusic);
            }
            sound.audioSource.Play();
            fadeIn(sound);
            backgroundMusic = sound;
        }
    }

    public void fadeOut(Sound sound)
    {
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }

        fadeOutCoroutine = StartCoroutine(startFade(sound, FadeDirection.FadeOut));
    }

    public void fadeIn(Sound sound)
    {
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
        }

        fadeInCoroutine = StartCoroutine(startFade(sound, FadeDirection.FadeIn));
    }

    private IEnumerator startFade(Sound sound, FadeDirection fadeDirection)
    {
        timeFaded = 0f;
        float startVolume = sound.audioSource.volume;

        while (timeFaded < fadeTime)
        {
            yield return fadeIntervalWait;
            timeFaded += fadeInterval;
            float volume = fadeDirection == FadeDirection.FadeIn
                ? Mathf.Lerp(0f, sound.volume, timeFaded / fadeTime)
                : Mathf.Lerp(startVolume, 0f, timeFaded / fadeTime);
            sound.audioSource.volume = volume;
        }

        if (fadeDirection == FadeDirection.FadeOut)
        {
            sound.audioSource.Stop();
        }
    }

    private enum FadeDirection
    {
        FadeIn,
        FadeOut
    }

    // TODO: Add a playOneShot method
    // TODO: Give a sound - play through an SFX audioSource

    // TODO: add a volume overload to oneShot method
}
