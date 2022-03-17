using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InspectionEvent : MusicPlayerEvent
{
    [SerializeField]
    [Tooltip("How often the inspection mode animation should switch from palm up to palm down")]
    private float animationPeriod = 3f;
    [SerializeField]
    GermMap germMap;
    [SerializeField]
    private InspectionMode inspectionMode;
    [SerializeField]
    private Transform rinseMenu;

    public bool isPalmsUp { get; private set; } = false;

    private WaitForSeconds animPeriodWait;
    private Coroutine animCoroutine = null;
    private InspectionStepButton currentGermLineButton = null;

    public override void SetupEvent()
    {
        base.SetupEvent();
        hasFinished = false;
        animPeriodWait = new WaitForSeconds(animationPeriod);
        isPalmsUp = false;
        //ToggleInspectAnimation(false);

        StartInspectionAnimation();

        if (!GermManager.instance.HasGermsOfType(GermType.Palm))
        {
            rinseMenu.DOScale(1f, 1f);
        }
    }

    private void StartInspectionAnimation()
    {
        //HandAnimations.instance.CrossFade("Idle", .2f);

        animCoroutine = StartCoroutine(FlipAnimation());
    }

    private void ToggleInspectAnimation()
    {
        isPalmsUp = !isPalmsUp;
        ToggleInspectAnimation(isPalmsUp);
    }

    private void ToggleInspectAnimation(bool status)
    {
        germMap.SetViewMode(status ? GermMap.HandViewMode.Up : GermMap.HandViewMode.Down);

        if (status)
        {
            HandAnimations.instance.CrossFade("Idle Up", .2f);
        }
        else
        {
            HandAnimations.instance.CrossFade("Idle", .2f);
        }
    }

    private IEnumerator FlipAnimation()
    {
        yield return animPeriodWait;
        ToggleInspectAnimation();
        animCoroutine = StartCoroutine(FlipAnimation());
    }


    public override void EndEvent()
    {
        base.EndEvent();
        if (animCoroutine != null)
        {
            StopCoroutine(animCoroutine);
        }
        animCoroutine = null;
    }

    public override bool ShouldRecord()
    {
        return false;
    }

    public override void DoEvent(Beat beat)
    {
    }

    public void SetGermLineButton(InspectionStepButton button)
    {
        currentGermLineButton = button;
    }

    public override void HardSwitchEnd()
    {
        EndEvent();
        inspectionMode.HardSwitchEnd();
    }

}
