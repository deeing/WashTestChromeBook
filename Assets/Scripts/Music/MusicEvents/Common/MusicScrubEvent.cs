using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;

public class MusicScrubEvent : MusicPlayerEvent
{
    [SerializeField]
    [Tooltip("Rhythym-synced inputs for this scrub event")]
    private RhythmInput rhythmInput;
    [SerializeField]
    private float scrubAnimationTime = 1f;
    [SerializeField]
    private string animationName;
    [SerializeField]
    private string returnAnimationName;
    [SerializeField]
    private string switchAnimationName;

    private int numBeatsInEvent = 0;

    private bool isPlayingEndAnimation = false;

    // Just testing, see if you want to make this a field
    private float endingOffset = -.5f;

    // latest updated rhythm input status for this beat
    private RhythmInputStatus latestRhythmInputStatus = RhythmInputStatus.Miss;

    private bool hardSwitching = false;

    public override void SetupEvent()
    {
        //MenuManager.instance.ShowScrubAlert(GetEventType().GetDescription(), 2f);
        base.SetupEvent();
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
        if (hardSwitching)
        {
            return;
        }

        if (numBeatsInEvent < numMeasures * MusicManager.instance.GetBeatsPerMeasure())
        {
            rhythmInput.DoBeat(beat, MusicManager.instance.GetNextBeat(beat));
            HandleScore();
        }
        else if (!isPlayingEndAnimation)
        {
            EndAnimation();
        }
        numBeatsInEvent++;
    }

    private void HandleScore()
    {
        float scoreAmount = MusicManager.instance.gameSettings.GetPointsForInputStatus(latestRhythmInputStatus);
        IncreaseEventScore(scoreAmount);
        MenuManager.instance.IncreaseTotalScore(scoreAmount);
        MenuManager.instance.ShowRhythmStatus(latestRhythmInputStatus);
        latestRhythmInputStatus = RhythmInputStatus.Miss;
    }

    public override void OnInput(RhythmInputStatus status)
    {
        if (hardSwitching)
        {
            return;
        }

        base.OnInput(status);

        if (status == RhythmInputStatus.Miss)
        {
            StopAnimation();
        }
        else
        {
            PlayAnimation();
        }

        UpdateRhythmInputStatus(status);
    }

    private void UpdateRhythmInputStatus(RhythmInputStatus status)
    {
        // only update if better than what was previously there
        // (otherwise a great score might overwrite a perfect one)
        latestRhythmInputStatus = status;
    }

    public override void EndEvent()
    {
        base.EndEvent();
        //EndAnimation();
    }

    public void StartAnimation()
    {
        HandAnimations.instance.PlayAnimation(animationName);
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
        MusicManager.instance.ToggleTransitioning(true);
        HandAnimations.instance.TransitionPlay(returnAnimationName);
        isPlayingEndAnimation = true;
        rhythmInput.Toggle(false);
        StartCoroutine(FinishScrub());
    }

    private IEnumerator FinishScrub()
    {
        yield return new WaitForSeconds(scrubAnimationTime + endingOffset);
        hasFinished = true;
    }

    public override MusicWashEvent GetNextWashEvent()
    {
        if (MusicManager.instance.nonLinearMode)
        {
            return MusicManager.instance.nonLinearCalSwitch;
        }
        return nextEvent;
    }

    public override void HardSwitchSetup()
    {
        hardSwitching = true;
        HandAnimations.instance.PlayAnimation(switchAnimationName);
        StartCoroutine(FinishHardSwitchSetup());
        rhythmInput.Toggle(true);
    }

    private IEnumerator FinishHardSwitchSetup()
    {
        yield return new WaitForSeconds(1.5f);
        hardSwitching = false;
        SetupEvent();
    }

    public override void HardSwitchEnd()
    {
        MusicManager.instance.ToggleTransitioning(true);
        HandAnimations.instance.Reset();
        HandAnimations.instance.TransitionPlay(returnAnimationName);
        rhythmInput.Toggle(false);
    }
}
