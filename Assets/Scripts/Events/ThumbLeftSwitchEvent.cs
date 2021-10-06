using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbLeftSwitchEvent : PlayerEvent
{
    [SerializeField]
    private float twistSensitivity = 0.05f;
    [SerializeField]
    [Tooltip("Total time it would take for transition animation to complete.")]
    private float transitionTime = 1f;
    [SerializeField]
    [Tooltip("Ending normalized time for the crossfade (figure out why this isn't just 1")]
    private float crossFadeLimit = .5f;

    public override void SetupEvent()
    {
        base.SetupEvent();
    }

    public override void DoEvent()
    {
        float twistAmount = Mathf.Abs(Lean.Touch.LeanGesture.GetTwistDegrees()) * twistSensitivity;

        if (twistAmount != 0)
        {
            float transitionTimeIncrease = Mathf.Abs(twistAmount) * twistSensitivity;
            TweenAnimation[] tweenAnimations = HandAnimations.instance.FindTweenAnimations(GetEventType());

            if (tweenAnimations != null)
            {
                HandAnimations.instance.CrossFadeTweenStep("Thumb Left Idle", tweenAnimations, transitionTime, transitionTimeIncrease, crossFadeLimit);
            } else
            {
                HandAnimations.instance.CrossFadeStep("Thumb Left Idle", transitionTime, transitionTimeIncrease, crossFadeLimit);
            }

        }
    }
    public override bool CheckEndEvent()
    {
        return HandAnimations.instance.IsCrossFadeWithTweensFinished();
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.ThumbLSwitch;
    }
}
