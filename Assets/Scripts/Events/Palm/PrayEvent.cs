using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrayEvent : SwitchEvent
{

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.PalmSwitch;
    }

    public override float DoTouchInput()
    {
        if (Lean.Touch.LeanGesture.GetPinchScale() < 1f)
        {
            float currFingerDistance = Lean.Touch.LeanGesture.GetScaledDistance();
            float lastFingerDistance = Lean.Touch.LeanGesture.GetLastScaledDistance();

            float pinchAmounnt = (lastFingerDistance - currFingerDistance);
            return pinchAmounnt;
        } else
        {
            return 0f;
        }
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Palm Switch", touchInputwithSensitivity);
    }
}
