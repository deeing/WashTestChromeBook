using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbRightSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.ThumbRSwitch;
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Thumb Right Switch", touchInputwithSensitivity);
    }
}
