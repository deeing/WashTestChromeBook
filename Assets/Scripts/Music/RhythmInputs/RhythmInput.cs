using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmTool;

public abstract class RhythymInput : MonoBehaviour
{
    private int beatCounter = 0;
    private int beatsPerinputPeriod = 0;
    private MusicPlayerEvent registeredEvent;

    private void Awake()
    {
        beatsPerinputPeriod = MusicManager.instance.GetBeatsPerInputPeriod();
    }

    public void Toggle(bool status)
    {
        gameObject.SetActive(status);
        beatCounter = 0;
    }

    private bool isOffBeat()
    {
        return beatCounter != 0;
    }

    public virtual void DoBeat(Beat currentBeat, Beat nextBeat)
    {
        if (!isOffBeat())
        {
            DoBeatWithoutOffBeat(currentBeat, nextBeat);
        }
        IncrementBeatCounter();
    }

    private void IncrementBeatCounter()
    {
        beatCounter++;

        if (beatCounter == beatsPerinputPeriod)
        {
            beatCounter = 0;
        }
    }

    public void RegisterWashEvent(MusicPlayerEvent musicPlayerEvent)
    {
        registeredEvent = musicPlayerEvent;
    }

    public void DoInput(bool status)
    {
        if (registeredEvent)
        {
            registeredEvent.OnInput(status);
        }
    }

    public abstract void DoBeatWithoutOffBeat(Beat currentBeat, Beat nextBeat);
}
