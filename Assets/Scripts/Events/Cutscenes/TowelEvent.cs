using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowelEvent : NonLinearAnimationSwitch
{
    protected override void EndAnimationEvent()
    {
        base.EndAnimationEvent();
        MusicManager.instance.EndGame();
    }
}
