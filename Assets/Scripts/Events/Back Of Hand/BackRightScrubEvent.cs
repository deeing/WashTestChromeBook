using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackRightScrubEvent : ScrubEvent
{
    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("Back Right Idle", idleTransitionTime);
    }

    public override void DoScrub()
    {
        //HandAnimations.instance.PlayAnimationStep("FingertipsScrub", touchInput);
        HandAnimations.instance.TransitionPlayStep("Back Right Scrub", idleTransitionTime, touchInputWithSensitivity);
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override GermType GetGermType()
    {
        return GermType.BackOfHandR;
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.BackOfHandRScrub;
    }

    public override void ReturnToNeutral()
    {
        HandAnimations.instance.PlayAnimation("Back Right Return");
    }
}
