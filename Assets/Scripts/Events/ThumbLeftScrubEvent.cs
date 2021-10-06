using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbLeftScrubEvent : ScrubEvent
{
    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("Thumb Left Idle", crossFadetime);
    }

    public override void DoScrub()
    {
        //HandAnimations.instance.PlayAnimationStep("FingertipsScrub", touchInput);
        HandAnimations.instance.TransitionPlay("Thumb Left Scrub", crossFadetime, touchInput);
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override GermType GetGermType()
    {
        return GermType.ThumbL;
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.ThumbLScrub;
    }

    public override void ReturnToNeutral()
    {
        HandAnimations.instance.PlayAnimation("Thumb Left Return", returnNeutralTime);
    }
}
