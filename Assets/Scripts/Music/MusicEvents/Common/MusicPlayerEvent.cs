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
    [SerializeField]
    private PlayerEventType eventType;

    protected bool hasFinished = false;
    protected float score = 0f;

    public abstract void DoEvent(Beat beat);
    public PlayerEventType GetEventType()
    {
        return eventType;
    }
    public abstract void SetupEvent();

    public virtual void OnInput(RhythmInputStatus status)
    {
        // What to do when receives player input
    }

    public virtual MusicWashEvent GetNextWashEvent()
    {
        return nextEvent;
    }

    public bool IsFinished()
    {
        return hasFinished;
    }

    public virtual void EndEvent()
    {
        // you don't have to end if you don't want to 
    }

    public float GetScore()
    {
        return score;
    }

    public void IncreaseEventScore(float updateAmount)
    {
        score += updateAmount;
    }

    public virtual bool ShouldRecord()
    {
        return true;
    }
}
