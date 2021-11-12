using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristRightSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.WristRSwitch;
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Wrist Right Switch", touchInputwithSensitivity);
    }
}
