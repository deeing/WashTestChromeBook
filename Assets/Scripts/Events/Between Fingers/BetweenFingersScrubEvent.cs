using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetweenFingersScrubEvent : ScrubEvent
{
    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("Between Fingers Idle", idleTransitionTime);
    }

    public override void DoScrub()
    {
        //HandAnimations.instance.PlayAnimationStep("FingertipsScrub", touchInput);
        HandAnimations.instance.TransitionPlayStep("Between Fingers Scrub", idleTransitionTime, touchInputWithSensitivity);
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override GermType GetGermType()
    {
        return GermType.BetweenFingers;
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.BetweenFingersScrub;
    }

    public override void ReturnToNeutral()
    {
        HandAnimations.instance.PlayAnimation("Between Fingers Return", returnNeutralTime);
    }
}
