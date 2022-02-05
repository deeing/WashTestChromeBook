using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonLinearAnimationSwitch : MusicSwitchEvent
{
    [SerializeField]
    private float animationTime = 1f;

    private WaitForSeconds animationWait;

    public override void SetupEvent()
    {
        animationWait = new WaitForSeconds(animationTime);
        StartEvent();
    }

    private void StartEvent()
    {
        //  HandAnimations.instance.CameraAnimation(cameraAnimationName, animationTime);
        StartCoroutine(FinishAnimation());
        HandAnimations.instance.PlayAnimation(animationName);
    }

    private IEnumerator FinishAnimation()
    {
        yield return animationWait;
        hasFinished = true;
    }

    public override void DoEvent(Beat beat)
    {
        // nothing
    }

}
