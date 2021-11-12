using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetweenFingersSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.BetweenFingersSwitch;
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Between Fingers Switch", touchInputwithSensitivity);
    }
}
