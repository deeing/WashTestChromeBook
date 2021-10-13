using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingertipsLeftSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.FingertipsLSwitch;
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Fingertips Left Switch", touchInputwithSensitivity);
    }
}
