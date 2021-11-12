using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristLeftSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.WristLSwitch;
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Wrist Left Switch", touchInputwithSensitivity);
    }
}
