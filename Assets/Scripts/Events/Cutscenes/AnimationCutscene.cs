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
    private string cameraAnimationName;

    public override void ChangeEvent()
    {
    }

    public override bool CheckEndEvent()
    {
        //return !HandAnimations.instance.IsPlayingAnimation(animationName);
        return false;
    }

    public override void DoEvent()
    {
        // playing video
    }

    public override void EndEvent()
    {
    }

    public override void SetupEvent()
    {
    }

    public override void StartEvent()
    {
       // HandAnimations.instance.PlayAnimation(animationName, animationTime);
      //  HandAnimations.instance.CameraAnimation(cameraAnimationName, animationTime);
    }
}
