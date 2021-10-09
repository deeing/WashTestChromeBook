using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingertipsScrubEvent : ScrubEvent
{
    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("FingertipsIdle", crossFadetime);
    }

    public override void DoScrub()
    {
        //HandAnimations.instance.PlayAnimationStep("FingertipsScrub", touchInput);
        HandAnimations.instance.TransitionPlay("FingertipsScrub", crossFadetime, touchInput);
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override GermType GetGermType()
    {
        return GermType.Fingertips;
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.FingertipsScrub;
    }

    public override void ReturnToNeutral()
    {
        HandAnimations.instance.CrossFade("Idle", returnNeutralTime);
    }
}
