using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristRightScrubEvent : ScrubEvent
{
    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("Wrist Right Idle", idleTransitionTime);
    }

    public override void DoScrub()
    {
        //HandAnimations.instance.PlayAnimationStep("FingertipsScrub", touchInput);
        HandAnimations.instance.TransitionPlayStep("Wrist Right Scrub", idleTransitionTime, touchInputWithSensitivity);
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override GermType GetGermType()
    {
        return GermType.WristR;
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.WristRScrub;
    }

    public override void ReturnToNeutral()
    {
        HandAnimations.instance.PlayAnimation("Wrist Right Return", returnNeutralTime);
    }
}
