using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackRightSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.BackOfHandRSwitch;
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Back Right Switch", touchInputwithSensitivity);
    }
}
