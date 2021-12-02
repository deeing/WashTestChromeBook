using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCaligraphySwitch : MusicSwitchEvent
{
    [SerializeField]
    private CaligraphyMove caligraphyMove;

    public override void SetupEvent()
    {
        CaligraphyInputManager.instance.ToggleCaligraphy(true);
        CaligraphyInputManager.instance.SetupGuideLines(caligraphyMove);
        CaligraphyInputManager.instance.ToggleInteractable(true);
    }

    public override void DoEvent(Beat beat)
    {
        // do nothing based on beats
    }
}
