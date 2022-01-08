using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonLinearCalSwitch : MusicPlayerEvent
{
    public void ChooseEvent(MusicSwitchEvent nextEvent)
    {
        this.nextEvent = nextEvent;
        hasFinished = true;
    }

    public override void SetupEvent()
    {
        base.SetupEvent();
        hasFinished = false;
        TogglePoseOptions(true);
        //HandAnimations.instance.CrossFade("Idle", .2f);

    }

    public override void EndEvent()
    {
        base.EndEvent();
        TogglePoseOptions(false);
    }

    private void TogglePoseOptions(bool status)
    {
        MenuManager.instance.TogglePoseOptions(status);
    }

    public override bool ShouldRecord()
    {
        return false;
    }

    public override void DoEvent(Beat beat)
    {
    }

    public override void HardSwitchSetup()
    {
        SetupEvent();
    }
}
