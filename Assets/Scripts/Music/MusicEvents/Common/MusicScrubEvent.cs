using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScrubEvent : MusicPlayerEvent
{
    [SerializeField]
    [Tooltip("Rhythym-synced inputs for this scrub event")]
    private RhythymInput rhythmInput;
    [SerializeField]
    private float scrubAnimationTime = 1f;
    [SerializeField]
    private string animationName;
    [SerializeField]
    private string returnAnimationName;

    private int numBeatsInEvent = 0;

    private bool isPlayingEndAnimation = false;

    public override void SetupEvent()
    {
        isPlayingEndAnimation = false;  
        numBeatsInEvent = 0;
        hasFinished = false;
        rhythmInput.Toggle(true);
        rhythmInput.RegisterWashEvent(this);
        StartAnimation();
        StopAnimation();
    }

    public override void DoEvent(Beat beat)
    {
        if (numBeatsInEvent < numMeasures * MusicManager.instance.GetBeatsPerMeasure())
        {
            rhythmInput.DoBeat(beat, MusicManager.instance.GetNextBeat(beat));
        } else if (!isPlayingEndAnimation)
        {
            EndAnimation();
        }
        numBeatsInEvent++;
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

    public override void EndEvent()
    {
        base.EndEvent();
        EndAnimation();
    }

    public void StartAnimation()
    {
        HandAnimations.instance.PlayAnimation(animationName, scrubAnimationTime);
    }
    public void PlayAnimation()
    {
        HandAnimations.instance.Resume();
    }
    public void StopAnimation()
    {
        HandAnimations.instance.Pause();
    }
    public void EndAnimation()
    {
        HandAnimations.instance.TransitionPlay(returnAnimationName);
        isPlayingEndAnimation = true;
        rhythmInput.Toggle(false);
        StartCoroutine(FinishScrub());
    }

    private IEnumerator FinishScrub()
    {
        yield return new WaitForSeconds(scrubAnimationTime);
        hasFinished = true;
    }

}
