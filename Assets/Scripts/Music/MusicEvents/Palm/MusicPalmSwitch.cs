using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPalmSwitch : MusicSwitchEvent
{
    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.PalmSwitch;
    }

    public override void PlaySwitchAnimation()
    {
        HandAnimations.instance.TransitionPlay("Palm Switch");
    }
}
