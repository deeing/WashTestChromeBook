using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristLeftScrubEvent : ScrubEvent
{
    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("Wrist Left Idle", idleTransitionTime);
    }

    public override void DoScrub()
    {
        //HandAnimations.instance.PlayAnimationStep("FingertipsScrub", touchInput);
        HandAnimations.instance.TransitionPlay("Wrist Left Scrub", idleTransitionTime, touchInput);
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override GermType GetGermType()
    {
        return GermType.WristL;
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.WristLScrub;
    }

    public override void ReturnToNeutral()
    {
        HandAnimations.instance.PlayAnimation("Wrist Left Return", returnNeutralTime);
    }
}
