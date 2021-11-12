using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackLeftSwitchEvent : SwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.BackOfHandLSwitch;
    }

    public override void DoSwitch()
    {
        HandAnimations.instance.PlayAnimationStep("Back Left Switch", touchInputwithSensitivity);
    }
}
