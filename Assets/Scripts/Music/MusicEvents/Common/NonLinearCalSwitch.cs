using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonLinearCalSwitch : MusicPlayerEvent
{
    [SerializeField]
    private LeftSlideMenu[] poseOptions;

    public void ChooseEvent(MusicSwitchEvent nextEvent)
    {
        this.nextEvent = nextEvent;
        hasFinished = true;
    }

    public override void SetupEvent()
    {
        hasFinished = false;
        TogglePoseOptions(true);
    }

    public override void EndEvent()
    {
        TogglePoseOptions(false);
    }

    private void TogglePoseOptions(bool status)
    {
        foreach (LeftSlideMenu poseOption in poseOptions)
        {
            poseOption.SetVisible(status);
        }
    }

    public override bool ShouldRecord()
    {
        return false;
    }

    public override void DoEvent(Beat beat)
    {
    }

}
