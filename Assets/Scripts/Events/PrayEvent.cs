using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrayEvent : PlayerEvent
{
    [SerializeField]
    private float pinchSensitivity = .002f;
    [SerializeField]
    [Tooltip("Total time it would take for transition animation to complete.")]
    private float transitionTime = 1f;
    [SerializeField]
    [Tooltip("Ending normalized time for the crossfade (figure out why this isn't just 1")]
    private float crossFadeLimit = .05f;    

    public override void DoEvent()
    {
        if (Lean.Touch.LeanGesture.GetPinchScale() < 1f)
        {
            float currFingerDistance = Lean.Touch.LeanGesture.GetScaledDistance();
            float lastFingerDistance = Lean.Touch.LeanGesture.GetLastScaledDistance();

            float pinchAmounnt = (lastFingerDistance - currFingerDistance);
            float transitionTimeIncrease = pinchAmounnt * pinchSensitivity;
            //HandAnimations.instance.PlayAnimationStep("Palm Idle", transitionTimeIncrease);
            HandAnimations.instance.CrossFadeStep("Palm Idle", transitionTime, transitionTimeIncrease, crossFadeLimit);
           // HandAnimations.instance.CrossFade("Palm Idle", transitionTime);
        }
    }
    public override bool CheckEndEvent()
    {
        return HandAnimations.instance.IsCrossFadeFinished();
        //return HandAnimations.instance.IsAnimationFinished();
    }

    public override string GetEventName()
    {
        return "Scrub Switch";
    }
}
