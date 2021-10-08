using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbRightSwitchEvent : PlayerEvent
{
    [SerializeField]
    private float twistSensitivity = 0.05f;
    [SerializeField]
    [Tooltip("Total time it would take for transition animation to complete.")]
    private float transitionTime = 1f;

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
            HandAnimations.instance.PlayAnimationStep("Thumb Right Switch", transitionTimeIncrease);

        }
    }
    public override bool CheckEndEvent()
    {
        return HandAnimations.instance.IsAnimationFinished();
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.ThumbRSwitch;
    }
}
