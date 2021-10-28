using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPalmScrubEvent : MusicScrubEvent
{
    public override void StartAnimation()
    {
        HandAnimations.instance.PlayAnimation("Palm Scrub", 1f);
    }
}
