using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackLeftScrubEvent : ScrubEvent
{
    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("Back Left Idle", crossFadetime);
    }

    public override void DoScrub()
    {
        //HandAnimations.instance.PlayAnimationStep("FingertipsScrub", touchInput);
        HandAnimations.instance.TransitionPlay("Back Left Scrub", crossFadetime, touchInput);
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
        HandAnimations.instance.CrossFade("Idle", returnNeutralTime);
    }
}
