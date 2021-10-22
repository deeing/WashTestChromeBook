using System.Collections;
using UnityEngine;

public abstract class SwitchEvent : PlayerEvent
{
    [SerializeField]
    private float sensitivity = .005f;
    public float touchInputwithSensitivity { get; private set; } = 0f;

    private bool isIdle = false;
    private bool unMoved = true;

    public abstract float DoTouchInput();

    public abstract void DoSwitch();
    public override void DoEvent()
    {
        touchInputwithSensitivity = DoTouchInput() * sensitivity * GetSensitivityAdjustment();

        if (touchInputwithSensitivity != 0)
        {
            DoSwitch();
            ResetImpatienceTimer();
            isIdle = false;
            unMoved = false;
        }
        else if(!isImpatient)
        {
            IncrementImpatienceTimer(Time.deltaTime);
            if (unMoved && impatienceTimer > impatienceThreshold)
            {
                HandleImpatience();
                isIdle = false;
            }
            else if (!isIdle) 
            {
                NeutralIdle();
                isIdle = true;
            }
        } 
    }

    private void NeutralIdle()
    {
        HandAnimations.instance.TransitionPlay("Idle", 1f, .2f);
    }

    protected override string GetImpatienceAnimationName()
    {
        return "Neutral Impatience";
    }

    public override bool CheckEndEvent()
    {
        return HandAnimations.instance.IsAnimationFinished();
    }
}
