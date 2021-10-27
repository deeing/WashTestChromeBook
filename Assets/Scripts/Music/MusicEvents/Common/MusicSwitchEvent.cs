using RhythmTool;
using System.Collections;
using UnityEngine;
using Wash.Utilities;

public abstract class MusicSwitchEvent :  MusicPlayerEvent
{
    [SerializeField]
    protected MusicWashEvent nextWashEvent;
    [SerializeField]
    protected float switchAnimationTime = 1f;


    public override void SetupEvent()
    {
        DisplayPoseOptions();
        ShowPrompt();
    }

    public void ShowPrompt()
    {
        MenuManager.instance.DisplaySwitchPrompt(GetEventType().GetDescription());
    }

    public void DisplayPoseOptions()
    {
        MenuManager.instance.DisplayPoseOptions(MusicManager.instance.starterEvents, this);
    }

    public void SuccessfulSwitch()
    {
        Debug.Log("YAY");
        EffectsManager.instance.Celebrate();
        DoSwitch();
    }

    public void FailedSwitch()
    {
        Debug.Log("Lol you suck");
        DoSwitch();
    }

    public void DoSwitch()
    {
        PlaySwitchAnimation();
        MenuManager.instance.HidePoseOptions();
        MenuManager.instance.HideSwitchPrompt();
        StartCoroutine(FinishSwitch());
    }
    
    private IEnumerator FinishSwitch()
    {
        yield return new WaitForSeconds(switchAnimationTime);
        hasFinished = true;
    }

    public abstract void PlaySwitchAnimation();

    public override void DoEvent(Beat beat)
    {

    }

    public override MusicWashEvent GetNextWashEvent()
    {
        return nextWashEvent;
    }
}
