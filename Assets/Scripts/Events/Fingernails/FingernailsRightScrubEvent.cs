using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingernailsRightScrubEvent : ScrubEvent
{
    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("Fingernails Right Idle", idleTransitionTime);
    }

    public override void DoScrub()
    {
        //HandAnimations.instance.PlayAnimationStep("FingertipsScrub", touchInput);
        HandAnimations.instance.TransitionPlay("Fingernails Right Scrub", idleTransitionTime, touchInput);
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override GermType GetGermType()
    {
        return GermType.FingernailsR;
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.FingernailsRScrub;
    }

    public override void ReturnToNeutral()
    {
        HandAnimations.instance.PlayAnimation("Fingernails Right Return", returnNeutralTime);
    }
}
