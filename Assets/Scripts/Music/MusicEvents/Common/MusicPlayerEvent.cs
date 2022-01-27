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
    public virtual void SetupEvent()
    {
        MusicManager.instance.ToggleTransitioning(false);
    }

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
        MusicManager.instance.ToggleTransitioning(true);
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

    // setup (for next event) that only needs to happen for a hard switch
    public virtual void HardSwitchSetup()
    {
        SetupEvent();
    }

    // end (for the prev event) that only needs to happen for a hard switch
    public virtual void HardSwitchEnd()
    {
        EndEvent();
    }
}
