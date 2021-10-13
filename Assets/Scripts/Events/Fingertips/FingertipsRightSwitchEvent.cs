using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingertipsRightSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.FingertipsRSwitch;
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Fingertips Right Switch", touchInputwithSensitivity);
    }
}
