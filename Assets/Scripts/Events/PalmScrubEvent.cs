using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmScrubEvent : ScrubEvent
{

    public override GermType GetGermType()
    {
        return GermType.PALM;
    }

    public override void DoScrub()
    {
        //HandAnimations.instance.PlayAnimationStep("Scrub", touchInput);
        HandAnimations.instance.TransitionPlay("Scrub", crossFadetime, touchInput);

    }

    public override void DoIdle()
    {
        HandAnimations.instance.CrossFade("Pray", crossFadetime);
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }
}
