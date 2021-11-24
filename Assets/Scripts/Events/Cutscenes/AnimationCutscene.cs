using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AnimationCutscene : CutsceneEvent
{
    [SerializeField]
    private string animationName;
    [SerializeField]
    private float animationTime = 1f;
    [SerializeField]
    private string soundName;

    private bool isFinished = false;

    private WaitForSeconds animationWait;

    public override void SetupEvent()
    {
        animationWait = new WaitForSeconds(animationTime);
    }

    public override void StartEvent()
    {
        //  HandAnimations.instance.CameraAnimation(cameraAnimationName, animationTime);
        StartCoroutine(FinishAnimation());
        HandAnimations.instance.PlayAnimation(animationName);
        AudioManager.instance.PlayOneShot(soundName);
    }

    private IEnumerator FinishAnimation()
    {
        yield return animationWait;
        isFinished = true;
    }

    public override void ChangeEvent()
    {
    }

    public override bool CheckEndEvent()
    {
        return isFinished;
    }

    public override void DoEvent()
    {
        // playing video
    }

    public override void EndEvent()
    {
    }


}
