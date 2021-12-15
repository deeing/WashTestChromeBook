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
    private string idleAnimationName;
    [SerializeField]
    private string scrubAnimationName;
    [SerializeField]
    private string returnAnimationName;
    [SerializeField]
    private string switchAnimationName;
    [SerializeField]
    private GermType germTypeKilled = GermType.Palm;
    [SerializeField]
    private float sensitivity = 1f;

    private int numBeatsInEvent = 0;

    private bool isPlayingEndAnimation = false;

    // Just testing, see if you want to make this a field
    private float endingOffset = -.5f;

    // latest updated rhythm input status for this beat
    private RhythmInputStatus latestRhythmInputStatus = RhythmInputStatus.Miss;

    private bool hardSwitching = false;
    private float idleTransitionTime = .2f;
    private float timeUntilIdle = 1f;
    private WaitForSeconds idleWait;
    private bool isIdle = false;
    private Coroutine idleCoroutine = null;

    private void Awake()
    {
        enabled = false;
        idleWait = new WaitForSeconds(timeUntilIdle);
    }

    public override void SetupEvent()
    {
        //MenuManager.instance.ShowScrubAlert(GetEventType().GetDescription(), 2f);
        base.SetupEvent();
        isPlayingEndAnimation = false;  
        numBeatsInEvent = 0;
        hasFinished = false;
        rhythmInput.Toggle(true);
        rhythmInput.RegisterWashEvent(this);
        HandAnimations.instance.PlayAnimation(idleAnimationName);
        HandAnimations.instance.Reset();
        enabled = true;
    }

    private void Update()
    {
        HandleTouch();
    }

    private void HandleTouch()
    {
        float input = Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
        if (input != 0)
        {
            EffectsManager.instance.ToggleBubbles(true);
            HandAnimations.instance.TransitionPlayStep(scrubAnimationName, idleTransitionTime, input * sensitivity);
            if (idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
                isIdle = false;
            }
        }
        else
        {
            if (idleCoroutine == null)
            {
                idleCoroutine = StartCoroutine(SetIdle());
            }
            else if (isIdle)
            {
                HandAnimations.instance.CrossFade(idleAnimationName, idleTransitionTime);
                EffectsManager.instance.ToggleBubbles(false);
            }
        }
    }

    private IEnumerator SetIdle()
    {
        yield return idleWait;
        isIdle = true;
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
            HandleGerms();
            HandleScore();
            ResetLatestRhythmInput();
        }
        else if (!isPlayingEndAnimation)
        {
            EndAnimation();
        }
        numBeatsInEvent++;
    }

    private void HandleGerms()
    {

        int numGermsToKill;
        switch (latestRhythmInputStatus)
        {
            case RhythmInputStatus.Good:
                numGermsToKill = 5;
                break;
            case RhythmInputStatus.Great:
                numGermsToKill = 10;
                break;
            case RhythmInputStatus.Perfect:
                numGermsToKill = 25;
                break;
            default:
                numGermsToKill = 0;
                break;
        }

        if (numGermsToKill > 0)
        {
            Debug.Log("Let's try and kill " + numGermsToKill + " germs");
            GermManager.instance.KillRandomGermsOfType(germTypeKilled, numGermsToKill);
        }
    }

    private void HandleScore()
    {
        float scoreAmount = MusicManager.instance.gameSettings.GetPointsForInputStatus(latestRhythmInputStatus);
        IncreaseEventScore(scoreAmount);
        MenuManager.instance.IncreaseTotalScore(scoreAmount);
        MenuManager.instance.ShowRhythmStatus(latestRhythmInputStatus);
    }

    private void ResetLatestRhythmInput()
    {
        latestRhythmInputStatus = RhythmInputStatus.Miss;
    }

    public override void OnInput(RhythmInputStatus status)
    {
        if (hardSwitching)
        {
            return;
        }

        base.OnInput(status);

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

    public void EndAnimation()
    {
        enabled = false;
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
        enabled = false;
    }
}
