using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MusicPlayerEvent : MonoBehaviour, MusicWashEvent
{
    [SerializeField]
    [Tooltip("How long this event should last in measures.")]
    protected int numMeasures = 2;
    
    protected bool hasFinished = false;

    public abstract void DoEvent(Beat beat);
    public abstract PlayerEventType GetEventType();
    public abstract void SetupEvent();
    public abstract MusicWashEvent GetNextWashEvent();

    public bool IsFinished()
    {
        return hasFinished;
    }

}
