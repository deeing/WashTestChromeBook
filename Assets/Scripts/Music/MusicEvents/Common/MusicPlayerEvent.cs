using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MusicPlayerEvent : MonoBehaviour, MusicWashEvent
{
    [SerializeField]
    [Tooltip("How long this event should last in measures.")]
    protected int numMeasures = 2;
    [SerializeField]
    protected MusicPlayerEvent nextEvent;

    protected bool hasFinished = false;

    public abstract void DoEvent(Beat beat);
    public abstract PlayerEventType GetEventType();
    public abstract void SetupEvent();

    public virtual void OnInput(bool status)
    {
        // What to do when receives player input
    }

    public MusicWashEvent GetNextWashEvent()
    {
        return nextEvent;
    }

    public bool IsFinished()
    {
        return hasFinished;
    }

}