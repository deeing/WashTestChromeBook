using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;
using Lean.Touch;

public class MusicScrubEvent : MusicPlayerEvent, AdjustableSensitivity
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
    private bool isNonLinearMode = false;
    private float sensitivityAdjustment = 1f;

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
        isNonLinearMode = MusicManager.instance.nonLinearMode;
        if (isNonLinearMode)
        {
            MenuManager.instance.ToggleFinishScrubButton(true);
        }
    }

    private void Update()
    {
        HandleTouch();
    }

    private void HandleTouch()
    {
        List<LeanFinger> fingers = LeanTouch.Fingers;
        if (fingers == null || fingers.Count == 0)
        {
            return;
        }

        float fingerMovement = LeanGesture.GetScaledDelta(fingers).magnitude;
        float input = fingerMovement * sensitivity * GetSensitivityAdjustment();

        if (input != 0)
        {
            EffectsManager.instance.ToggleBubbles(true);
            HandAnimations.instance.TransitionPlayStep(scrubAnimationName, idleTransitionTime, input);
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

        if (isNonLinearMode || numBeatsInEvent < numMeasures * MusicManager.instance.GetBeatsPerMeasure())
        {
            rhythmInput.DoBeat(beat, MusicManager.instance.GetNextBeat(beat));
            HandleGerms();
            HandleScore();
            ResetLatestRhythmInput();
        }
        else if (!isPlayingEndAnimation && !isNonLinearMode)
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
                numGermsToKill = 10;
                break;
            case RhythmInputStatus.Great:
                numGermsToKill = 25;
                break;
            case RhythmInputStatus.Perfect:
                numGermsToKill = 50;
                break;
            default:
                numGermsToKill = 0;
                break;
        }

        if (numGermsToKill > 0)
        {
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
        MenuManager.instance.ToggleFinishScrubButton(false);
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
        MenuManager.instance.ToggleFinishScrubButton(false);
        enabled = false;
    }

    public void SetSensitivityAdjustment(float sensitivityAdjustment)
    {
        this.sensitivityAdjustment = sensitivityAdjustment;
    }

    public float GetSensitivityAdjustment()
    {
        return sensitivityAdjustment;
    }

    public string GetEventName()
    {
        return scrubAnimationName;
    }
}
