using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonLinearCalSwitch : MusicPlayerEvent
{
    [SerializeField,
        Tooltip("How long it pauses before applying the neutral idle animation")]
    private float neutralIdleDelay = 1f;

    private WaitForSeconds neutralIdleWait;

    private void Awake()
    {
        neutralIdleWait = new WaitForSeconds(neutralIdleDelay);
    }

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
        StartCoroutine(PlayNeutralIdleAnimation());
    }

    public override void EndEvent()
    {
        base.EndEvent();
        TogglePoseOptions(false);
    }

    private void TogglePoseOptions(bool status)
    {
        MenuManager.instance.ToggleNonLinearSwitchOptions(status);
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

    private IEnumerator PlayNeutralIdleAnimation()
    {
        yield return neutralIdleWait;
        HandAnimations.instance.PlayAnimation("Idle");
    }
}
