using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetweenFingersSwitchEvent : PlayerEvent
{
    [SerializeField]
    private float pinchSensitivity = .05f;
    [SerializeField]
    [Tooltip("Total time it would take for transition animation to complete.")]
    private float transitionTime = 1f;

    public override void SetupEvent()
    {
        base.SetupEvent();
    }

    public override void DoEvent()
    {
        if (Lean.Touch.LeanGesture.GetPinchScale() < 1f)
        {
            float currFingerDistance = Lean.Touch.LeanGesture.GetScaledDistance();
            float lastFingerDistance = Lean.Touch.LeanGesture.GetLastScaledDistance();

            float pinchAmounnt = (lastFingerDistance - currFingerDistance);
            float transitionTimeIncrease = pinchAmounnt * pinchSensitivity;
            HandAnimations.instance.PlayAnimationStep("Between Fingers Switch", transitionTimeIncrease);
        }
    }
    public override bool CheckEndEvent()
    {
        return HandAnimations.instance.IsAnimationFinished();
    }

    public override PlayerEventType GetEventType()
    {
        return PlayerEventType.BetweenFingersSwitch;
    }
}
