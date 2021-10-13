using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetweenFingersSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.BetweenFingersSwitch;
    }

    public override float DoTouchInput()
    {
        if (Lean.Touch.LeanGesture.GetPinchScale() < 1f)
        {
            float currFingerDistance = Lean.Touch.LeanGesture.GetScaledDistance();
            float lastFingerDistance = Lean.Touch.LeanGesture.GetLastScaledDistance();

            float pinchAmounnt = (lastFingerDistance - currFingerDistance);
            return pinchAmounnt;
        }

        return 0;
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Between Fingers Switch", touchInputwithSensitivity);
    }
}
