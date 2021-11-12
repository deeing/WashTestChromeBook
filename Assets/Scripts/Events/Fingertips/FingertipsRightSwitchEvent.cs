using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingertipsRightSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.FingertipsRSwitch;
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Fingertips Right Switch", touchInputwithSensitivity);
    }
}
