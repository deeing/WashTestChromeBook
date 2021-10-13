using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackLeftScrubEvent : ScrubEvent
{
    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("Back Left Idle", idleTransitionTime);
    }

    public override void DoScrub()
    {
        //HandAnimations.instance.PlayAnimationStep("FingertipsScrub", touchInput);
        HandAnimations.instance.TransitionPlay("Back Left Scrub", idleTransitionTime, touchInput);
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override GermType GetGermType()
    {
        return GermType.BackOfHandL;
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.BackOfHandLScrub;
    }

    public override void ReturnToNeutral()
    {
        HandAnimations.instance.PlayAnimation("Back Left Return", returnNeutralTime);
    }
}
