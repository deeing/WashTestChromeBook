using System.Collections;
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
            Setup();
        }
        else {
            Destroy(this);
        }
    }

    private void Setup() {
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

    private Sound FindSound(string soundName)
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

    public bool SoundIsPlaying(string soundName)
    {
        return FindSound(soundName).audioSource.isPlaying;
    }

    // TODO: Maybe convert private dictionary to make more effecient
    public void PlaySound(string soundName) {
        Sound sound = FindSound(soundName);
        if (sound != null)
        {
            sound.audioSource.Play();
        }
    }

    public void PlayOneShot(string soundName)
    {
        Sound sound = FindSound(soundName);
        if (sound != null)
        {
            sound.audioSource.PlayOneShot(sound.audioSource.clip);
        }
    }

    public void PlayBackground(string soundName)
    {
        Sound sound = FindSound(soundName);
        if (sound != null && sound != backgroundMusic)
        {
            if(backgroundMusic != null)
            {
                FadeOut(backgroundMusic);
            }
            sound.audioSource.Play();
            FadeIn(sound);
            backgroundMusic = sound;
        }
    }

    public void FadeOut(string soundName)
    {
        Sound sound = FindSound(soundName);
        if (sound != null)
        {
            FadeOut(sound);
        }
    }

    public void FadeOut(Sound sound)
    {
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }

        fadeOutCoroutine = StartCoroutine(StartFade(sound, FadeDirection.FadeOut));
    }

    public void FadeIn(Sound sound)
    {
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
        }

        fadeInCoroutine = StartCoroutine(StartFade(sound, FadeDirection.FadeIn));
    }

    private IEnumerator StartFade(Sound sound, FadeDirection fadeDirection)
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
