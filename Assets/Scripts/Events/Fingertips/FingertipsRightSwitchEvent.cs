using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingertipsRightSwitchEvent : PlayerEvent
{
    [SerializeField]
    private float twistSensitivity = .1f;

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
            HandAnimations.instance.PlayAnimationStep("Fingertips Right Switch", transitionTimeIncrease);
        }
    }
    public override bool CheckEndEvent()
    {
        return HandAnimations.instance.IsAnimationFinished();
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.FingertipsRSwitch;
    }
}
