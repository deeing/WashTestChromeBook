using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmScrubEvent : ScrubEvent
{

    public override GermType GetGermType()
    {
        return GermType.Palm;
    }

    public override void DoScrub()
    {
        //HandAnimations.instance.PlayAnimationStep("Palm Scrub", touchInput);
        //HandAnimations.instance.TransitionPlay("Palm Scrub", "Palm Together", crossFadetime, touchInput);
        HandAnimations.instance.TransitionPlay("Palm Scrub", idleTransitionTime, touchInput);

    }

    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("Palm Idle", idleTransitionTime);
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.PalmScrub;
    }

    public override void ReturnToNeutral()
    {
        HandAnimations.instance.CrossFade("Idle", returnNeutralTime);
    }
}
