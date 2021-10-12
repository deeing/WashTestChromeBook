using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingertipsRightScrubEvent : ScrubEvent
{
    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("Fingertips Right Idle", crossFadetime);
    }

    public override void DoScrub()
    {
        HandAnimations.instance.TransitionPlay("Fingertips Right Scrub", crossFadetime, touchInput);
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override GermType GetGermType()
    {
        return GermType.FingertipsR;
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.FingertipsRScrub;
    }

    public override void ReturnToNeutral()
    {
        HandAnimations.instance.PlayAnimation("Fingertips Right Return", returnNeutralTime);
    }
}
