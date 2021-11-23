using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingertipsLeftScrubEvent : ScrubEvent
{
    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("Fingertips Left Idle", idleTransitionTime);
    }

    public override void DoScrub()
    {
        HandAnimations.instance.TransitionPlayStep("Fingertips Left Scrub", idleTransitionTime, touchInputWithSensitivity);
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override GermType GetGermType()
    {
        return GermType.FingertipsL;
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.FingertipsLScrub;
    }

    public override void ReturnToNeutral()
    {
        HandAnimations.instance.PlayAnimation("Fingertips Left Return");
    }
}
