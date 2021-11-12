using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingernailsLeftSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.FingernailsLSwitch;
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Fingernails Left Switch", touchInputwithSensitivity);
    }
}
