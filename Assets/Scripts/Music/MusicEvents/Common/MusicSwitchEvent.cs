using RhythmTool;
using System.Collections;
using UnityEngine;
using Wash.Utilities;

public class MusicSwitchEvent :  MusicPlayerEvent
{
    [SerializeField]
    protected float switchAnimationTime = 1f;
    [SerializeField]
    private string animationName;

    private bool eventStarted = false;
    private bool isStarterEvent = false;

    public override void SetupEvent()
    {
        hasFinished = false;
        eventStarted = false;

        if (isStarterEvent)
        {
            DisplayPoseOptions();
        } else
        {
            DisplayPoseOptions(nextEvent);
        }
    }

    public void ShowPrompt(Beat beat)
    {
        string text = GetEventType().GetDescription();
        float promptTime = GetPromptTravelTime(beat);
        MenuManager.instance.DisplaySwitchPrompt(text, promptTime);
    }

    private float GetPromptTravelTime(Beat beat)
    {
        float currentBeatTime = beat.timestamp;
        Beat nextBeat = MusicManager.instance.GetNextBeat(beat, numMeasures * MusicManager.instance.GetBeatsPerMeasure());
        float nextBeatTime = nextBeat.timestamp;
        return nextBeatTime - currentBeatTime;
    }

    public void DisplayPoseOptions()
    {
        MenuManager.instance.DisplayPoseOptions(MusicManager.instance.starterEvents, this);
    }

    public void DisplayPoseOptions(MusicWashEvent nextEvent)
    {
        MenuManager.instance.DisplayPoseOptions(nextEvent, this);
    }

    public void SuccessfulSwitch()
    {
        EffectsManager.instance.Celebrate();
        DoSwitch();
    }

    public void FailedSwitch()
    {
        DoSwitch();
    }

    public void DoSwitch()
    {
        PlaySwitchAnimation();
        MenuManager.instance.HidePoseOptions();
        MenuManager.instance.HideSwitchPrompt();
        StartCoroutine(FinishSwitch());
    }
    
    private IEnumerator FinishSwitch()
    {
        yield return new WaitForSeconds(switchAnimationTime);
        hasFinished = true;
    }

    public void PlaySwitchAnimation()
    {
        HandAnimations.instance.TransitionPlay(animationName);
    }

    public override void DoEvent(Beat beat)
    {
        if (!eventStarted)
        {
            ShowPrompt(beat);
            eventStarted = true;
        }
    }

    public void RegisterAsStartEvent()
    {
        isStarterEvent = true;
    }
}
