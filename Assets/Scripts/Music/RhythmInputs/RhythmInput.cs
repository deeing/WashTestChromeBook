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

    private bool IsOffBeat()
    {
        return beatCounter != 0;
    }

    public virtual void DoBeat(Beat currentBeat, Beat nextBeat)
    {
        if (!IsOffBeat())
        {
            HandleBeat(currentBeat, nextBeat);
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

    public void DoInput(RhythmInputStatus status)
    {
        if (registeredEvent)
        {
            registeredEvent.OnInput(status);
        }
    }

    public void DoInputMiss()
    {
        DoInput(RhythmInputStatus.Miss);
    }

    public void DoInputGood()
    {
        DoInput(RhythmInputStatus.Good);
    }

    public void DoInputGreat()
    {
        DoInput(RhythmInputStatus.Great);
    }

    public void DoInputPerfect()
    {
        DoInput(RhythmInputStatus.Perfect);
    }

    public abstract void HandleBeat(Beat currentBeat, Beat nextBeat);
}
