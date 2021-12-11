using RhythmTool;
using System.Collections;
using UnityEngine;
using Wash.Utilities;

public class MusicSwitchEvent :  MusicPlayerEvent
{
    [SerializeField]
    protected float switchAnimationTime = 1f;
    [SerializeField]
    protected string animationName;

    private bool eventStarted = false;
    private bool isStarterEvent = false;

    // Just testing, see if you want to make this a field
    private float endingOffset = -.5f;
    private int numPoseOptions = 4; // maybe this should be a gamesetting?

    /*public override void SetupEvent()
    {
        hasFinished = false;
        eventStarted = false;

        if (isStarterEvent)
        {
            DisplayPoseOptions(numPoseOptions);
        } else
        {
            DisplayPoseOptions(this);
        }
    }*/

    public virtual void ShowPrompt(Beat beat)
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

    public void DisplayPoseOptions(int numPoseOptions)
    {
        MenuManager.instance.DisplayPoseOptions(MusicManager.instance.starterEvents, this, numPoseOptions);
    }

    public void DisplayPoseOptions(MusicSwitchEvent currentSwitchEvent)
    {
        MenuManager.instance.DisplayPoseOptions(currentSwitchEvent);
    }

    public void SuccessfulSwitch(RhythmInputStatus inputStatus)
    {
        // accolades
        MenuManager.instance.ShowAlert(inputStatus.GetDescription(), 1f);
        EffectsManager.instance.Celebrate();
        // score
        float scoreAmount = MusicManager.instance.gameSettings.GetPointsForInputStatus(inputStatus);
        IncreaseEventScore(scoreAmount);
        MenuManager.instance.IncreaseTotalScore(scoreAmount);
        
        DoSwitch();
    }

    public void FailedSwitch()
    {
        MenuManager.instance.ShowAlert("Wrong option", 1f);
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
        yield return new WaitForSeconds(switchAnimationTime + endingOffset);
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

    public override bool ShouldRecord ()
    {
        return false;
    }
}
