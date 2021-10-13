using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingernailsRightSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.FingernailsRSwitch;
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Fingernails Right Switch", touchInputwithSensitivity);
    }
}
