using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingernailsRightSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.FingernailsRSwitch;
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Fingernails Right Switch", touchInputwithSensitivity);
    }
}
