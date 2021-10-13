using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbLeftSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.ThumbLSwitch;
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Thumb Left Switch", touchInputwithSensitivity);
    }
}
