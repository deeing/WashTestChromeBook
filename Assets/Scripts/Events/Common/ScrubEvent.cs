using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;

public abstract class ScrubEvent : PlayerEvent, AdjustableSensitivity
{
    [SerializeField]
    [Tooltip("If user gives no input after this time, we transition to the idle animation")]
    private float idleTime = .5f;
    [SerializeField]
    [Tooltip("How long the idle transition animation should take.")]
    protected float idleTransitionTime = .25f;
    [SerializeField]
    private SwitchEvent relativeSwitch;
    [SerializeField]
    private bool finishWhenGermsGone = false;
    
    
    protected float scrubbingSpeedMinMult = .25f;
    protected float scrubbingSpeedMaxMult = 2f;
    protected float scrubbingSpeedIncreaseRate = 1.01f;
    private float scrubBubbleRateMax = 125;
    private float bubbleSpeedMax = 5f;

    public float touchInputWithSensitivity { get; private set; } = 0f;
    private WaitForSeconds idleWait;
    private Coroutine idleCoroutine;
    private bool isIdle = false;
    private GermType germType;
    private string switchAnimName;
    private bool isReturningFromInspect = false;
    private float sensitivityAdjustment = 1f;
    private float scrubbingSpeedMult;

    public override void SetupEvent()
    {
        base.SetupEvent();
        idleWait = new WaitForSeconds(idleTime);
        germType = GetGermType();

        switchAnimName = relativeSwitch.caligraphyMove.animationName;
        //GermManager.instance.ShowGermBar(germType);
        scrubbingSpeedMult = scrubbingSpeedMinMult;
        MenuManager.instance.ToggleScrubPowerBar(true);
        ResetScrubbingSpeed();
    }

    public override bool CheckEndEvent()
    {
        return finishWhenGermsGone && !GermManager.instance.HasGermsOfType(germType);
    }

    public abstract GermType GetGermType();
    public abstract void DoScrub();

    public abstract void DoIdle();

    public abstract void ReturnToNeutral();

    public abstract float DoTouchInput();
    public float HandleInput()
    {
        if (WashEventManager.instance.isInspectionMode)
        {
            return 0;
        }
        else
        {
            return DoTouchInput() * sensitivity * GetSensitivityAdjustment() * GetSpeedAdjustment(); ;
        }
    }


    public override void DoEvent()
    {
        if (WashEventManager.instance.isInspectionMode)
        {
            return;
        }

        if (isReturningFromInspect)
        {
            HandAnimations.instance.PlayAnimationStep(switchAnimName, Time.deltaTime);

            if (HandAnimations.instance.IsAnimationFinished())
            {
                HandAnimations.instance.Reset();
                isReturningFromInspect = false;
            }
            return;
        }

        touchInputWithSensitivity = HandleInput();
        EffectsManager.instance.PlaySuds(GetEventType());

        if (touchInputWithSensitivity > 0)
        {
            GermManager.instance.KillRandomGermOfType(germType);
            EffectsManager.instance.ToggleBubbles(true);
            DoScrub();
            if (idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
                isIdle = false;
                isImpatient = false;
                ResetImpatienceTimer();
            }
            IncreaseScrubbingSpeed();
        } else
        {
            if (idleCoroutine == null)
            {
                idleCoroutine = StartCoroutine(SetIdle());
            }
            else if (isIdle && !isImpatient)
            {
                IncrementImpatienceTimer(Time.deltaTime);
                if (HasImpatienceAnim() && impatienceTimer > impatienceThreshold)
                {
                    HandleImpatience();
                }
                else
                {
                    DoIdle();
                    ResetScrubbingSpeed();
                    EffectsManager.instance.ToggleBubbles(false);
                }
            }
        }
    }

    private IEnumerator SetIdle()
    {
        yield return idleWait;
        isIdle = true;
    }

    public override void EndEvent()
    {
        base.EndEvent();
        //GermManager.instance.HideGermBar();
        //ReturnToNeutral();
    }

    public override void ChangeEvent()
    {
        base.ChangeEvent();
        //HandAnimations.instance.Reset();
        GermManager.instance.HideGermBar();
        MenuManager.instance.ToggleScrubPowerBar(false);
        ReturnToNeutral();
    }

    public override void ReturnFromInspect()
    {
        isReturningFromInspect = true;
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

    private float GetSpeedAdjustment()
    {
        //Debug.Log("Scrubbing speed mult is " + scrubbingSpeedMult);
        // increase scrubbing speed over time
        return scrubbingSpeedMult;
    }

    private void ResetScrubbingSpeed()
    {
        scrubbingSpeedMult = scrubbingSpeedMinMult;
        MenuManager.instance.SetScrubPowerPercentage(0f);
        EffectsManager.instance.SetBubbleEmission(0f);
        EffectsManager.instance.SetBubbleSpeed(1f);
    }

    private void IncreaseScrubbingSpeed()
    {
        scrubbingSpeedMult *= scrubbingSpeedIncreaseRate;
        scrubbingSpeedMult.ClampUpper(scrubbingSpeedMaxMult);
        MenuManager.instance.SetScrubPowerPercentage(CalcScrubPowerPecent());
        EffectsManager.instance.SetBubbleEmission(CalcScrubPowerPecent() * scrubBubbleRateMax);
        EffectsManager.instance.SetBubbleSpeed(1 + (CalcScrubPowerPecent() * (bubbleSpeedMax-1)));
    }

    private float CalcScrubPowerPecent()
    {
        float maxPower = scrubbingSpeedMaxMult - scrubbingSpeedMinMult;
        float currPower = scrubbingSpeedMult - scrubbingSpeedMinMult;

        return currPower / maxPower;
    }
}
