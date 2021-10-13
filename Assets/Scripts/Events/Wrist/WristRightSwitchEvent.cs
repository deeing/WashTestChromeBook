using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristRightSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.WristRSwitch;
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Wrist Right Switch", touchInputwithSensitivity);
    }
}
