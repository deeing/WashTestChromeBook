using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingertipsSwitchEvent : PlayerEvent
{
    [SerializeField]
    private float twistSensitivity = .002f;
    [SerializeField]
    [Tooltip("Total time it would take for transition animation to complete.")]
    private float transitionTime = 1f;
    [SerializeField]
    [Tooltip("Ending normalized time for the crossfade (figure out why this isn't just 1")]
    private float crossFadeLimit = .05f;

    public override void SetupEvent()
    {
        base.SetupEvent();
        //HandAnimations.instance.CrossFade("Idle", 1f);
    }

    public override void DoEvent()
    {
        float twistAmount = Lean.Touch.LeanGesture.GetTwistDegrees();

        if (twistAmount != 0)
        {
            float transitionTimeIncrease = twistAmount * twistSensitivity;
            //HandAnimations.instance.CrossFadeStep("FingertipsIdle", transitionTime, transitionTimeIncrease, crossFadeLimit);
            HandAnimations.instance.PlayAnimationStep("ScrubToFingertipsSwitch", transitionTimeIncrease);
        }
    }
    public override bool CheckEndEvent()
    {
        return HandAnimations.instance.IsAnimationFinished();
    }
}
