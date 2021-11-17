using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScrubEvent : PlayerEvent
{
    [SerializeField]
    [Tooltip("If user gives no input after this time, we transition to the idle animation")]
    private float idleTime = .5f;
    [SerializeField]
    [Tooltip("How long the idle transition animation should take.")]
    protected float idleTransitionTime = .25f;
    [SerializeField]
    [Tooltip("Time for return to neutral animation after scrubbing finishes")]
    protected float returnNeutralTime = .5f;

    public float touchInputWithSensitivity { get; private set; } = 0f;
    private WaitForSeconds idleWait;
    private Coroutine idleCoroutine;
    private bool isIdle = false;
    private GermType germType;
    private bool isFinished = false;
    private string switchAnimName;
    private bool isReturningFromInspect = false;

    public override void SetupEvent()
    {
        base.SetupEvent();
        idleWait = new WaitForSeconds(idleTime);
        germType = GetGermType();

        SwitchEvent relativeSwitch = (SwitchEvent)WashEventManager.instance.GetSwitchEvent(this);
        switchAnimName = relativeSwitch.caligraphyMove.animationName;
        //GermManager.instance.ShowGermBar(germType);
    }

    public override bool CheckEndEvent()
    {
        return isFinished;
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
            return DoTouchInput();
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
            EffectsManager.instance.Bubbles();
            DoScrub();
            if (idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
                isIdle = false;
                isImpatient = false;
                ResetImpatienceTimer();
            }
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
        GermManager.instance.HideGermBar();
        ReturnToNeutral();
    }

    public override void ChangeEvent()
    {
        base.ChangeEvent();
        GermManager.instance.HideGermBar();
        ReturnToNeutral();
    }

    public void FinishEvent()
    {
        isFinished = true;
    }

    public override void ReturnFromInspect()
    {
        isReturningFromInspect = true;
    }
}
