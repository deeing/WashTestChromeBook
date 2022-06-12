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
    [SerializeField]
    private bool showBubbles = true;
    [SerializeField]
    private MusicScrubEvent otherHand;

    // how much fire affets the score of the scrub
    private const float FIRE_SCORE_MULT = 2;

    // how often we score and clean germs in seconds
    private const float SCORING_PERIOD = .2f;
    private float currentScoreWait = 0f;

    private bool isPlayingEndAnimation = false;

    // Just testing, see if you want to make this a field
    private float endingOffset = -.5f;

    // latest updated rhythm input status for this beat
    protected RhythmInputStatus latestRhythmInputStatus = RhythmInputStatus.Miss;

    private bool hardSwitching = false;
    private float idleTransitionTime = .2f;
    private float timeUntilIdle = 1f;
    private WaitForSeconds idleWait;
    private bool isIdle = false;
    private Coroutine idleCoroutine = null;
    private bool isNonLinearMode = false;
    private float sensitivityAdjustment = 1f;
    private bool isDisplayingTip = false;
    private bool isFireDoubleEvent = false;

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
        hasFinished = false;
        rhythmInput.Toggle(true);
        rhythmInput.RegisterWashEvent(this);
        HandAnimations.instance.PlayAnimation(idleAnimationName);
        HandAnimations.instance.Reset();
        enabled = true;
        isNonLinearMode = MusicManager.instance.nonLinearMode;

        MenuManager.instance.ToggleFinishScrubButton(true);
        if (otherHand != null)
        {
            MenuManager.instance.ToggleSwitchHandsButton(true);
        }

        if (MusicManager.instance.fireDoubleScrubEvent == this)
        {
            EffectsManager.instance.ToggleFire(true);
            isFireDoubleEvent = true;
        }
        MusicManager.instance.ToggleCrosshairSystem(true, rhythmInput);
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
            if (showBubbles)
            {
                EffectsManager.instance.ToggleBubbles(true);
            }
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

        rhythmInput.DoBeat(beat, MusicManager.instance.GetNextBeat(beat));
        //HandleGerms();
        //HandleScore();
        //ResetLatestRhythmInput();

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

        if (isNonLinearMode && 
            !isDisplayingTip && 
            germTypeKilled != GermType.NO_TYPE &&
            !GermManager.instance.HasGermsOfType(germTypeKilled) &&
            !HintManager.instance.hasUsedInspect)
        {
            HintManager.instance.ToggleInspectHintMenu(true);
            isDisplayingTip = true;
        }
    }

    protected virtual void HandleScore()
    {
        // don't score if there are no germs left to scrub
        if (!GermManager.instance.HasGermsOfType(germTypeKilled))
        {
            return;
        }
        //Debug.Log("Scoring for " + latestRhythmInputStatus);

        float scoreAmount = MusicManager.instance.gameSettings.GetPointsForInputStatus(latestRhythmInputStatus);
        if (isFireDoubleEvent)
        {
            scoreAmount *= FIRE_SCORE_MULT;
        }
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

        currentScoreWait += Time.deltaTime;
        if(currentScoreWait >= SCORING_PERIOD)
        {
            HandleGerms();
            HandleScore();
            currentScoreWait = 0f;
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
        HintManager.instance.ToggleInspectHintMenu(false);
    }

    public virtual void EndAnimation()
    {
        enabled = false;
        MusicManager.instance.ToggleTransitioning(true);
        //HandAnimations.instance.TransitionPlay(returnAnimationName);
        HandAnimations.instance.PlayAnimation(returnAnimationName);
        isPlayingEndAnimation = true;
        rhythmInput.Toggle(false);
        MenuManager.instance.ToggleFinishScrubButton(false);
        MenuManager.instance.ToggleSwitchHandsButton(false);
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
        //HandAnimations.instance.TransitionPlay(returnAnimationName);
        HandAnimations.instance.PlayAnimation(returnAnimationName);
        rhythmInput.Toggle(false);
        MenuManager.instance.ToggleFinishScrubButton(false);
        MenuManager.instance.ToggleSwitchHandsButton(false);
        HintManager.instance.ToggleInspectHintMenu(false);
        enabled = false;
        EffectsManager.instance.ToggleFire(false);
        MusicManager.instance.ToggleCrosshairSystem(false, null);
    }

    public void SetSensitivityAdjustment(float sensitivityAdjustment)
    {
        this.sensitivityAdjustment = sensitivityAdjustment;
    }

    public float GetSensitivityAdjustment()
    {
        return sensitivityAdjustment;
    }

    public float GetBaseSensitivity()
    {
        return sensitivity;
    }

    public string GetEventName()
    {
        return scrubAnimationName;
    }

    public void SwitchToOtherHand()
    {
        HardSwitchEnd();
        HandAnimations.instance.PlayAnimation(returnAnimationName);
        StartCoroutine(DoHandSwitch());
    }

    private IEnumerator DoHandSwitch()
    {
        yield return new WaitForSeconds(1f);
        MusicManager.instance.HardSwitchEvent(otherHand);
    }
}
