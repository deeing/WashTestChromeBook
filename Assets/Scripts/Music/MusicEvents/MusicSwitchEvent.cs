using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;

public abstract class MusicSwitchEvent : MonoBehaviour, MusicWashEvent
{
    [SerializeField]
    protected MusicWashEvent nextWashEvent;
    [SerializeField]
    protected float switchAnimationTime = 1f;

    bool hasFinished = false;

    public void SetupEvent()
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
        StartCoroutine(FinishSwitch());
    }
    
    private IEnumerator FinishSwitch()
    {
        yield return new WaitForSeconds(switchAnimationTime);
        hasFinished = true;
    }

    public abstract void PlaySwitchAnimation();

    public void DoEvent(Beat beat)
    {
        if (hasFinished)
        {
            FinishEvent();
        } 
    }

    private void FinishEvent()
    {
        MusicManager.instance.ChangeEvent(nextWashEvent);
        MusicManager.instance.RemoveStarterSwitchEvent(this);
    }


    public abstract PlayerEventType GetEventType();
}
