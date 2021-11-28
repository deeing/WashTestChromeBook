using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : SingletonMonoBehaviour<EffectsManager>
{
    [SerializeField]
    private ParticleSystem celebrationParticles;
    [SerializeField]
    private ParticleSystem bubblesParticles;
    [SerializeField]
    private PlayerEventToSudsMapping[] playerEventToSuds;
    [SerializeField]
    private GameObject[] fires;
    [SerializeField]
    private GameObject defaultLights;
    [SerializeField]
    private GameObject uvLights;

    private Dictionary<PlayerEventType, ParticleSystem> playerEventsToSudsMap;
    // suds systems that are currently playing
    private HashSet<PlayerEventType> activeSudsSystems;

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }

        playerEventsToSudsMap = new Dictionary<PlayerEventType, ParticleSystem>();
        activeSudsSystems = new HashSet<PlayerEventType>();
        for(int i=0; i < playerEventToSuds.Length; i++)
        {
            PlayerEventToSudsMapping mapping = playerEventToSuds[i];
            playerEventsToSudsMap.Add(mapping.playerEvent, mapping.sudsParticleSystem);
        }
    }

    public void Celebrate()
    {
        celebrationParticles.Play();
    }

    public void ToggleBubbles(bool status)
    {
        if (status)
        {
            StartBubbles();
        } else
        {
            EndBubbles();
        }
    }

    public void SetBubbleEmission(float emissionRate)
    {
        ParticleSystem.EmissionModule emission = bubblesParticles.emission;
        emission.rateOverTime = emissionRate;
    }

    public void SetBubbleSpeed(float speed)
    {
        ParticleSystem.MainModule particleMain = bubblesParticles.main;
        particleMain.simulationSpeed = speed;
    }


    private void StartBubbles()
    {
        if (!bubblesParticles.isPlaying)
        {
            bubblesParticles.Play();
        }
    }

    private void EndBubbles()
    {
        if (bubblesParticles.isPlaying)
        {
            bubblesParticles.Stop();
        }
    }

    public void PlaySuds(PlayerEventType playerEventType)
    {
        if (playerEventsToSudsMap.ContainsKey(playerEventType)) {
            ParticleSystem particleSystem = playerEventsToSudsMap[playerEventType];
            if (!particleSystem.isPlaying)
            {
                particleSystem.Play();
                activeSudsSystems.Add(playerEventType);
            }
        }
    }

    // shows or hides the suds
    public void ToggleSuds(bool status)
    {
        foreach(PlayerEventType playerEventType in activeSudsSystems)
        {
            ParticleSystem particleSystem = playerEventsToSudsMap[playerEventType];
            if (status)
            {
                particleSystem.gameObject.SetActive(true);
                particleSystem.Play();
            } else
            {
                particleSystem.Pause();
                particleSystem.gameObject.SetActive(false);
            }
        }
    }

    // shows or hides the fire
    public void ToggleFire(bool status)
    {
        foreach(GameObject fire in fires)
        {
            fire.SetActive(status);
        }
    }

    public void ToggleUvLights(bool status)
    {
        defaultLights.SetActive(!status);
        uvLights.SetActive(status);
    }

}
