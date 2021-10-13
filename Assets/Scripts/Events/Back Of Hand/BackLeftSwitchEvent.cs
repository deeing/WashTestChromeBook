using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackLeftSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.BackOfHandLSwitch;
    }

    public override float DoTouchInput()
    {
        return Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees());
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Back Left Switch", touchInputwithSensitivity);
    }
}
