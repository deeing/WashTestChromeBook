using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbRightScrubEvent : ScrubEvent
{
    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("Thumb Right Idle", crossFadetime);
    }

    public override void DoScrub()
    {
        //HandAnimations.instance.PlayAnimationStep("FingertipsScrub", touchInput);
        HandAnimations.instance.TransitionPlay("Thumb Right Scrub", crossFadetime, touchInput);
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override GermType GetGermType()
    {
        return GermType.ThumbR;
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.ThumbRScrub;
    }

    public override void ReturnToNeutral()
    {
        HandAnimations.instance.PlayAnimation("Thumb Right Return", returnNeutralTime);
    }
}