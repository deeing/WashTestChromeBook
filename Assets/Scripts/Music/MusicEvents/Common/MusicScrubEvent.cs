using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MusicScrubEvent : MusicPlayerEvent
{
    [SerializeField]
    [Tooltip("Rhythym-synced inputs for this scrub event")]
    private RhythymInput rhythmInput;

    public override void SetupEvent()
    {
        rhythmInput.Toggle(true);
        rhythmInput.RegisterWashEvent(this);
        StartAnimation();
        StopAnimation();
    }

    public override void DoEvent(Beat beat)
    {
        rhythmInput.DoBeat(beat, MusicManager.instance.GetNextBeat(beat));
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.PalmScrub;
    }

    public override void OnInput(bool status)
    {
        base.OnInput(status);

        if (status)
        {
            PlayAnimation();
        } else
        {
            StopAnimation();
        }
    }

    public abstract void StartAnimation();
    public void PlayAnimation()
    {
        HandAnimations.instance.Resume();
    }
    public void StopAnimation()
    {
        HandAnimations.instance.Pause();
    }
}
