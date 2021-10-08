using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingernailsLeftScrubEvent : ScrubEvent
{
    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("Fingernails Left Idle", crossFadetime);
    }

    public override void DoScrub()
    {
        //HandAnimations.instance.PlayAnimationStep("FingertipsScrub", touchInput);
        HandAnimations.instance.TransitionPlay("Fingernails Left Scrub", crossFadetime, touchInput);
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override GermType GetGermType()
    {
        return GermType.FingernailsL;
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.FingernailsLScrub;
    }

    public override void ReturnToNeutral()
    {
        HandAnimations.instance.PlayAnimation("Fingernails Left Return", returnNeutralTime);
    }
}
